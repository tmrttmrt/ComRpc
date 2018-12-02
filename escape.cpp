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

#include <Arduino.h>

inline char* c2h(char c, bool capital=false){
	static char buff[3];
	const size_t a = capital ? 'A' - 1 : 'a' - 1;

	buff[0] = c > 0x9F ? ((c >> 4) - 9) | a : (c >> 4)  | '0';
	buff[1] = (c & 0xF) > 9 ? ((c & 0xF) - 9) | a : (c & 0xF) | '0';
	buff[3]=0;
	return buff;
}

inline char h2c(char* hexstr){
	char c;

	c = (*hexstr & '@' ? *hexstr + 9: *hexstr ) << 4;
	hexstr++;
	c |= (*hexstr & '@' ? *hexstr + 9 : *hexstr) & 0xF;
	return c;
}


size_t escapeString(const char * str, char * estr){
	char *buff;
	size_t i=0;
	while(*str){
		switch (*str){
			case '\\':
			case '\r':
			case '\n':
			case ',':
			case ':':
			case ' ':
				if(estr){
					*estr++='\\';
					buff=c2h(*str++);
					*estr++=*buff++;
					*estr++=*buff;
				} else {
					str++;
				}
				i+=3;
				break;
			default:
				if(estr){
					*estr++=*str++;
				} else {
					str++;
				}
				i++;
				break;
		}
	}
	if(estr) *estr=0;
	return i;
}

char* unescapeString(char * estr){
	char *str=estr;
	char *dest=estr;
	
	while(*estr){
		switch (*estr){
			case '\\':
				*dest++=h2c(++estr);
				estr++;
				estr++;
				break;
			default:
				*dest++=*estr++;
				break;
		}
	}
	*dest=0;
	return str;
}