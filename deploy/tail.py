#!/usr/bin/env python
# coding=utf-8
# https://github.com/kasun/python-tail/blob/master/tail.py

'''
Python-Tail - Unix tail follow implementation in Python.
python-tail can be used to monitor changes to a file.
Example:
    import tail
    # Create a tail instance
    t = tail.Tail('file-to-be-followed')
    # Register a callback function to be called when a new line is found in the followed file.
    # If no callback function is registerd, new lines would be printed to standard out.
    t.register_callback(callback_function)
    # Follow the file with 5 seconds as sleep time between iterations.
    # If sleep time is not provided 1 second is used as the default time.
    t.follow(s=5) '''

# Author - Kasun Herath <kasunh01 at gmail.com>
# Source - https://github.com/kasun/python-tail

import os
import sys
import time


class Tail(object):
    ''' Represents a tail command. '''

    def __init__(self, tailed_file):
        ''' Initiate a Tail instance.
            Check for file validity, assigns callback function to standard out.

            Arguments:
                tailed_file - File to be followed. '''

        self._running = True
        self.check_file_validity(tailed_file)
        self.tailed_file = tailed_file
        self.callback = sys.stdout.write

    def follow(self, s=1):
        ''' Do a tail follow. If a callback function is registered it is called with every new line.
        Else printed to standard out.

        Arguments:
            s - Number of seconds to wait between each iteration; Defaults to 1. '''

        with open(self.tailed_file) as file_:
            # Go to the end of file
            file_.seek(0, 2)
            while self._running:
                curr_position = file_.tell()
                line = file_.readline()
                if not line:
                    file_.seek(curr_position)
                    time.sleep(s)
                else:
                    self.callback(line)

            line = file_.readline()
            if line:
                self.callback(line)

    def terminate(self):
        self._running = False

    def register_callback(self, func):
        ''' Overrides default callback function to provided function. '''
        self.callback = func

    def check_file_validity(self, file_):
        ''' Check whether the a given file exists, readable and is a file '''
        if not os.access(file_, os.F_OK):
            raise TailError("File '%s' does not exist" % (file_))
        if not os.access(file_, os.R_OK):
            raise TailError("File '%s' not readable" % (file_))
        if os.path.isdir(file_):
            raise TailError("File '%s' is a directory" % (file_))


class TailError(Exception):

    def __init__(self, msg):
        Exception.__init__(self)
        self.message = msg

    def __str__(self):
        return self.message