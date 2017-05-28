# -*- coding:utf-8 -*-
'''
Created on 2017年5月30日

@author: jia
'''
import logging
import os
import shutil
import subprocess
import sys

import log
from xutils import env, timethis

log.init_logger('table')
logger = logging.getLogger('table')
sys.stderr = log.ErrOutPutToLogger("table")


class CmdError(Exception):
    pass


@timethis(logger.info)
def run_cmd(command, desc=None):
    if desc:
        logger.info('>>>>>>>> %s 开始', desc)

    ret = subprocess.call(command, shell=True)
    if ret != 0:
        cmd_str = command
        if isinstance(command, list):
            cmd_str = ' '.join(command)
        message = "Error running command: %s", cmd_str
        raise CmdError(message)

    if desc:
        logger.info('>>>>>>>> %s 结束', desc)


def unity_export():
    logger.info(' 导出unity android 项目 '.center(80, '-'))
    logger.info('步骤1：清理文件夹')

    unity_proj_path = os.path.join(env('EXPORT_PROJECT_PATH'), env('UNITY_PROJECT_NAME'))
    logger.info('清理unity android项目文件夹: %s', unity_proj_path)
    if os.path.exists(unity_proj_path):
        shutil.rmtree(unity_proj_path)

    logger.info('步骤2：调用UNITY打包')
    logger.info('UNITY_PATH: %s', env('UNITY_PATH'))
    logger.info('UNITY_LOG_PATH: %s', env('UNITY_LOG_PATH'))
    logger.info('UNITY_PROJECT_PATH: %s', env('UNITY_PROJECT_PATH'))
    logger.info('UNITY_METHOD_NAME: %s', env('UNITY_METHOD_NAME'))
    logger.info('EXPORT_PROJECT_PATH: %s', env('EXPORT_PROJECT_PATH'))
    cmd = [env('UNITY_PATH'), '-quit', '-batchmode', '-logFile', env('UNITY_LOG_PATH'), '-projectPath', env('UNITY_PROJECT_PATH'), '-executeMethod', env('UNITY_METHOD_NAME'), '-outputPath', env('EXPORT_PROJECT_PATH')]
    run_cmd(cmd, 'unity打包导出')

    logger.info('步骤3：删除unity android项目中的application定义')
    gradle_file = os.path.join(unity_proj_path, 'build.gradle')
    gradle_file_bak = os.path.join(unity_proj_path, 'build.gradle.bak')
    data = ''
    with open(gradle_file, 'r') as fd:
        data = fd.readlines()
    with open(gradle_file_bak, 'w') as fd:
        fd.write(''.join(data))
    data = [l.rstrip() for l in data]
    data = [l.replace('\t', ' ' * 4) for l in data]
    text = '\n'.join(data) + '\n'
    # apply plugin: 'com.android.application' 修改为 apply plugin: 'com.android.library'
    text = text.replace('apply plugin: \'com.android.application\'', 'apply plugin: \'com.android.library\'')
    # 注释掉这句代码 applicationId "com.xxx.xxxx"
    idx = text.find('applicationId')
    if idx > 0 and (text[idx - 1] == ' ' or text[idx - 1] == '\n') and text[idx + len('applicationId')] == ' ':
        text = text.replace('applicationId', '// applicationId', 1)
    with open(gradle_file, 'w') as fd:
        fd.write(text)
    return


def main():
    import argparse

    parser = argparse.ArgumentParser(description='deploy.')
    parser.add_argument('--unity-build', action='store_true', help="unity生成apk")
    parser.add_argument('--unity-export', action='store_true', help="unity导出project")
    args = parser.parse_args()
    print(args)
    if args.unity_export:
        unity_export()


if __name__ == '__main__':
    main()
