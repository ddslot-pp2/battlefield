#!/usr/bin/env python3
#-*- coding: utf-8 -*-

import xml.etree.ElementTree as ET
import os
import subprocess
import fnmatch
import sys
import operator
try:
    from configparser import ConfigParser
except ImportError:
    from ConfigParser import ConfigParser

g_packet_xml_file_path = None
g_protoc_folder_path  = None

# create ini file
def create_ini_file():
	cfgfile = open("packet_config.ini",'w')
	config = ConfigParser()

	# parse existing file
	config.read('packet_config.ini')

	config.add_section('IN')
	path = os.getcwd()

	config.set('IN', 'protoc_folder_path', path)
	config.set('IN', 'packet_xml_file_path', path + '/packet.xml')
	# set default out path
	config.add_section('OUT')
	path = os.getcwd()

	config.set('OUT', 'opcode_folder_path', path + '/Assets/Network/Opcode')
	config.set('OUT', 'packet_folder_path', path + '/Assets/Network/Packet')

	config.write(cfgfile)

	cfgfile.close()

# check ini file
def check_ini_file():
	#print ('#1 There is no setting_packet.ini')
	if not os.path.exists('packet_config.ini'):
		return False
	return True


def check_xml_path():
	config = ConfigParser()
	config.read('packet_config.ini')
	# read values from a section
	xml_path = config.get('IN', 'packet_xml_file_path')
	protoc_path = config.get('IN', 'protoc_folder_path')

	if not os.path.exists(xml_path):
		return False

	global g_packet_xml_file_path
	g_packet_xml_file_path = xml_path
	print(g_packet_xml_file_path)

	global g_protoc_folder_path
	g_protoc_folder_path = protoc_path
	return True
	

def create_opcode():
	config = ConfigParser()
	config.read('packet_config.ini')
	# read values from a section
	opcode_path = config.get('OUT', 'opcode_folder_path')

	print (opcode_path)
	#return
	# 폴더가 존재하는지 여부 ㅎ확인 후 만들기

	global g_packet_xml_file_path
	
	packet_xml = ET.parse(g_packet_xml_file_path)
	root = packet_xml.getroot()

	target = open(opcode_path + '/' + 'opcode.cs', 'w')
	
	target.write('public enum opcode : short\n')
	target.write('{\n')


	value = 0
	for child in root:
		value = child.attrib['start']
		for packet in child:
			if 'type' not in packet.attrib and 'struct' not in packet.attrib:
				target.write('\t' + packet.tag + ' = ' + str(value) + ',\n')
				value = int(value) + 1


	target.write('};\n')
	
	target.close()
# ----------------------------------
# .proto 생성
# ----------------------------------
def create_proto():
	print ('create .proto')
	global g_packet_xml_file_path
	
	packet_xml = ET.parse(g_packet_xml_file_path)
	root = packet_xml.getroot()

	for package in root:
		target = open(package.tag + '.proto', 'w')

		target.write('syntax = "proto3";\n')
		target.write('\n')
		target.write('package ' + package.tag + ';\n')
		target.write('\n')
		
		for packet in package:
			# packet
			if 'type' not in packet.attrib:
					print (packet.tag)

					child_count = 1
					target.write('message ' + packet.tag + '\n')
					target.write('{\n')

					for child in packet:
						if 'repeated' in child.attrib:
							target.write('\t' + 'repeated ' + child.attrib['type'] + ' ' + child.tag + ' = ' +  str(child_count) + ';\n');
						else:
							target.write('\t' + child.attrib['type'] + ' ' + child.tag + ' = ' +  str(child_count) + ';\n');
						child_count = child_count + 1
						
					target.write('}\n')
					target.write('\n')
			else:
				if 'enum' == packet.attrib['type'].lower():
				
					target.write('enum ' + packet.tag + '\n')
					target.write('{\n')

					enum_count = 0
					for child in packet:
						target.write('\t' + child.tag + ' = ' +  str(enum_count) + ';\n');
						enum_count = enum_count + 1


					target.write('}\n')
					target.write('\n')

		target.close()


def create_packet():

	config = ConfigParser()
	config.read('packet_config.ini')
	packet_out_folder = config.get('OUT', 'packet_folder_path')


	current_path = os.getcwd()
	protos = []

	files = [f for f in os.listdir('.') if os.path.isfile(f)]
	for f in files:
	    extension = os.path.splitext(f)[1][1:]
	    if extension == 'proto':
	        protos.append(f);


	os.chdir(g_protoc_folder_path)
	for proto in protos:
		print(proto)
		#cmd =  "protoc "  + ' -I="../../../../../data/protobuf" --cpp_out="../../../../../sgs2/src/packet_processor/packet" ' +  "../../../../../data/protobuf/" + proto
		#os.system(cmd)
		#protoc -I=$SRC_DIR --csharp_out=$DST_DIR $SRC_DIR/addressbook.proto
		SRC_DIR = current_path
		DST_DIR = packet_out_folder

		cmd = "protoc"  + ' -I=' + SRC_DIR + ' --csharp_out=' + DST_DIR + ' ' + SRC_DIR + '/' + proto

		print(cmd)
		os.system(cmd)
	    #cmd =  "protoc "  + ' -I="../../../../../proto" --csharp_out="../../../../../proto/csharp_out/packet" ' +  "../../../../../proto/" + proto
	    #os.system(cmd)

	os.chdir(current_path)

def clean_proto():
	protos = []

	files = [f for f in os.listdir('.') if os.path.isfile(f)]
	for f in files:
	    extension = os.path.splitext(f)[1][1:]
	    if extension == 'proto':
	        protos.append(f);
	        
	print ('delete .proto')
	for proto in protos:
		os.remove('./' + proto)

def main():
	if not check_ini_file():
		create_ini_file()
		#print('Please set packet_xml_path in packet_config.ini file!')

	if not check_xml_path():
		print('Please set packet_xml_path in packet_config.ini file!')
		return
	
	create_proto()
	create_opcode()
	create_packet()
	clean_proto()

if __name__ == "__main__":
	main()