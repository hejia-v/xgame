# -*- coding:utf-8 -*-
'''
Created on 2017年5月30日

@author: jia
'''
import os
import time
from functools import wraps


def env(name):
    value = os.environ[name]
    value = value.strip()
    if value.startswith('\'') and value.startswith('\''):
        value = value[1:-1]
    if value.startswith('\"') and value.startswith('\"'):
        value = value[1:-1]
    if os.path.isdir(value) or os.path.isfile(value) or os.path.isabs(value):
        assert(os.path.isabs(value))
        value = os.path.normpath(value)
    return value


def timethis(logger=None):
    '''
    Decorator that reports the execution time.
    '''
    def decorate(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            start = time.time()
            result = func(*args, **kwargs)
            end = time.time()
            summary = '%s %s: %s' % (func.__name__, '耗时', end - start)
            if logger:
                logger(summary)
            else:
                print(summary)
            return result
        return wrapper
    return decorate
