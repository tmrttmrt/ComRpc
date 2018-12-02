A lightweight Arduino to C# serial RPC library.

The transfer is not the most efficient, but more lightweight than json.
Numeric types are transfered in the decimal ASCII representation.
char* is transfered in a custom escaped string.

Usage

A utility convert.exe writen in C# based on Vitaliy Kleban's tiny-rpc https://bitbucket.org/sources/tiny-rpc code parses prototype definitions in a cpp file and generates cpp server code and  C# code that provide communication. 


cpp code generation command line:

convert.exe -server [input_cpp_file] > [output_cpp_file]

C# code generation output to a file:

convert.exe -cs_client [input_cpp_file] > [output_cs_file]