# a2po
Converter to convert android xml resources files to .po format and back

# Description
This tool allows you to manage your android translations using *.po and *.pot files
It's important do not delete reference comments from *.po files, because it's used to restoer android ids.
Tool do not overwrite strings.xml files when converting from *.po, it opens it and updates if it's possible. So if you have some comments in xml, it will remain there..
Also if you don't want to make string translatable don't forget to mark it by translatable="false" attribute.
This tool now supports only android resources located in "strings.xml", other files will not be read/written

# Instructions
-x:[XML_PATH] - path to android project root directory (where src folder is located)
-p:[PO_PATH]  - path to location of po files
-a:[COMMAND]  - action command: 
        x2p   - transfers all xml data to PO files
        p2x   - transfers all po data to project XML files
        clean - cleans all saved temp files in android project
-t            - save previous version of replacable files (in ~string.xml or in ~lan.po)

