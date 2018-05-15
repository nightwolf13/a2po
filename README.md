﻿# a2po
Converter to convert android xml resources files to .po format and back

# Description
This tool allows you to manage your android translations using *.po and *.pot files
It's important do not delete reference comments from *.po files, because it's used to restore android ids.
Also if you don't want to make string translatable don't forget to mark it by translatable="false" attribute.
This tool supports only android resources located in "strings.xml", other files will not be read/written

# Instructions
-x:[XML_PATH] - path to android project root directory (where src folder is located)\
-p:[PO_PATH]  - path to location of po files\
-a:[COMMAND]  - action command:\
-t            - save previous version of replacable files (in ~string.xml or in ~lan.po)\
-m            - path to txt file of language map between android and po resources\
-is           - ignore android source file (values\strings.xml) when p2x command is used

Commands:\
x2p		- transfers all xml data to PO files\
p2x		- transfers all po data to project XML files\
clean	- cleans all saved temp files in android project

Format of ignore file:\
(android language code)=(po language code)\
For example:\
zh=zh_TW\
en=en_US