#include <Arduino.h>
#include "ComRpc.h"

uint32_t atouint32_t(char *str){
	return (uint32_t) strtoul(str,0,0);
}