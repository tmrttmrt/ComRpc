/*
	Copyright (C) 2018 T. Mertelj
	
	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/


#include "ComRpc.h"

char *returnCString();
char *testCString(char *cstring);
double testDouble(double par);
float testFloat(float par);
int8_t testChar(int8_t cin);
int16_t testInt16(int16_t par);
int32_t testInt32(int32_t par);
uint8_t testUChar(uint8_t cin);
uint16_t testUInt16(uint16_t par);
uint32_t testUInt32(uint32_t par);
void testVoid();

void testVoid(){

}

char *returnCString(){
  return "Test return string.";
}


char *testCString(char *cstring){
//  Serial.print(cstring);
  return cstring;
}

int8_t testChar(int8_t inc){
  return inc;
}

uint8_t testUChar(uint8_t inc){
  return inc;
}

int16_t testInt16(int16_t inc){
  return inc;
}

uint16_t testUInt16(uint16_t inc){
  return inc;
}

int32_t testInt32(int32_t inc){
  return inc;
}

uint32_t testUInt32(uint32_t inc){
  return inc;
}


double testDouble(double par){
  return 10*par;  
}

float testFloat(float par){
  return 10*par;  
}


void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);

}


String line_string;

void loop() {
  
  // put your main code here, to run repeatedly:
  line_string=Serial.readStringUntil('\r');
  line_string.trim();
  if(line_string.length()==0){
    return;
  }
  Serial.println(line_string);
  if(line_string[0]==':'){ //ComRpc
    Serial.println(rpc_proc_line((char*) line_string.c_str()));
    line_string="";
    return;
  }

}
