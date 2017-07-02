# -*- coding:utf-8 -*-
'''
Created on 2017年5月30日

@author: jia
'''
import logging
import os
import re
import shutil
import subprocess
import sys
from threading import Thread
from functools import partial

import log
import tail
import xutils
from xutils import env, timethis, file_diff, print_diff

log.init_logger('table')
logger = logging.getLogger('table')
sys.stderr = log.ErrOutPutToLogger("table")


class CmdError(Exception):
    pass


@timethis(logger.info)
def run_cmd(command, desc=None, cwd=None, callback=None):
    if desc:
        logger.info('>>>>>>>> %s 开始', desc)

    ret = subprocess.call(command, shell=True, cwd=cwd)
    if ret != 0:
        cmd_str = command
        if isinstance(command, list):
            cmd_str = ' '.join(command)
        message = "Error running command: %s", cmd_str
        raise CmdError(message)

    if callback:
        callback()

    if desc:
        logger.info('>>>>>>>> %s 结束', desc)


def tail_thread(_tail: tail.Tail):
    logger.info("wait for tail file ... %s", _tail.tailed_file)

    while True:
        if os.path.exists(_tail.tailed_file):
            logger.info("Start tail file..... %s", _tail.tailed_file)
            break

    _tail.register_callback(unity_log_tail)
    _tail.follow(s=0.5)


def unity_log_tail(txt):
    print(txt.rstrip())


def stop_tail(_tail: tail.Tail, t: Thread):
    _tail.terminate()
    t.join()
    logger.info("Stop tail file..... %s", _tail.tailed_file)


def unity_export():
    logger.info(' 调用UNITY打包 '.center(80, '-'))
    logger.info('UNITY_PATH: %s', env('UNITY_PATH'))
    logger.info('UNITY_LOG_PATH: %s', env('UNITY_LOG_PATH'))
    logger.info('UNITY_PROJECT_PATH: %s', env('UNITY_PROJECT_PATH'))
    logger.info('UNITY_METHOD_NAME: %s', env('UNITY_METHOD_NAME'))
    logger.info('EXPORT_PROJECT_PATH: %s', env('EXPORT_PROJECT_PATH'))

    unity_log_path = env('UNITY_LOG_PATH')
    logger.info('清理 unity 编译log: %s', unity_log_path)
    with open(unity_log_path, 'w') as fd:
        fd.write('')

    cmd = [env('UNITY_PATH'), '-quit', '-batchmode', '-logFile', env('UNITY_LOG_PATH'), '-projectPath',
           env('UNITY_PROJECT_PATH'), '-executeMethod', env('UNITY_METHOD_NAME'), '-outputPath', env('EXPORT_PROJECT_PATH')]

    # new thread to tail log file
    _tail = tail.Tail(unity_log_path)
    t = Thread(target=tail_thread, args=(_tail, ))
    t.start()
    run_cmd(cmd, 'unity打包导出', None, partial(stop_tail, _tail, t))


def modify_unity_project():
    logger.info(' 删除unity android项目中的application定义 '.center(80, '-'))

    unity_proj_path = os.path.join(env('EXPORT_PROJECT_PATH'), env('UNITY_PROJECT_NAME'))

    gradle_file = os.path.join(unity_proj_path, 'build.gradle')
    gradle_file_old = os.path.join(unity_proj_path, 'build.gradle.old')
    logger.info('修改gradle文件: %s', gradle_file)

    xutils.file_normalize(gradle_file)
    xutils.fcopy(gradle_file, gradle_file_old)

    text = ''
    with open(gradle_file, 'r') as fd:
        text = fd.read()

    # apply plugin: 'com.android.application' 修改为 apply plugin: 'com.android.library'
    text = text.replace('apply plugin: \'com.android.application\'',
                        'apply plugin: \'com.android.library\'')
    # 注释掉这句代码 applicationId "com.xxx.xxxx"
    patt = re.compile('\s+applicationId\s+')
    def f(m):
        s = m.group()
        return s.replace('applicationId', '// applicationId', 1)
    text = patt.sub(f, text, 1)
    with open(gradle_file, 'w') as fd:
        fd.write(text)

    modifyDiff = file_diff(gradle_file_old, gradle_file)
    print_diff(modifyDiff)


def clean_project():
    logger.info(' 清理文件夹 '.center(80, '-'))

    unity_proj_path = os.path.join(env('EXPORT_PROJECT_PATH'), env('UNITY_PROJECT_NAME'))
    logger.info('清理unity android项目文件夹: %s', unity_proj_path)
    if os.path.exists(unity_proj_path):
        shutil.rmtree(unity_proj_path)


def build_apk():
    logger.info(' build android apk '.center(80, '-'))
    android_proj_path = env('ANDROID_PROJECT_PATH')
    cmd = ['gradlew', 'assembleRelease']
    run_cmd(cmd, 'build android apk', android_proj_path)


def android_deploy(is_build_apk: bool):
    clean_project()
    unity_export()
    modify_unity_project()
    if is_build_apk:
        build_apk()


def main():
    import argparse

    parser = argparse.ArgumentParser(description='deploy.')
    parser.add_argument(
        '--android-deploy', action='store_true', help="unity android deploy")
    parser.add_argument(
        '-a', '--build-apk', action='store_true', help="编译 android apk")
    args = parser.parse_args()
    print(args)
    if args.android_deploy:
        android_deploy(args.build_apk)
    # TODO: unity的c#文件中的platform.cs作为配置文件，打包时修改其中的配置，打包完毕后还原


if __name__ == '__main__':
    main()
