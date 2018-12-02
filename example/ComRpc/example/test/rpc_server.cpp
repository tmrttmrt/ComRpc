#include <Arduino.h>
#include "DIYRpc.h"

String response; // Using WString class for a clever static buffer that can (hopefully) grow only.

extern char* returnCString();
extern char* testCString(char* cstring);
extern double testDouble(double par);
extern float testFloat(float par);
extern int8_t testChar(int8_t cin);
extern int16_t testInt16(int16_t par);
extern int32_t testInt32(int32_t par);
extern uint8_t testUChar(uint8_t cin);
extern uint16_t testUInt16(uint16_t par);
extern uint32_t testUInt32(uint32_t par);
extern void testVoid();

char* rpc_proc_line( char *line ){
	
	if(!response.reserve(RET_BUFF_SIZE)){ //To prevent heap fragmentation we should reserve large enough buffer on the first call. 
		response=F(":1,Out of memory!");
		return response.c_str();
	} 
	char *pc=strtok(line+1,",");
	if(NULL==pc){
		response=F(":1, func_id missing");
		return response.c_str();
	}
	int id=atoi(pc);

	switch (id){

		case 0:	// char* returnCString(...) 
		{

			char* ret_returnCString = returnCString();
			response = F(":0,");
			if(response.reserve(response.length()+escapeString(ret_returnCString,NULL)+1)){
				escapeString(ret_returnCString, response.c_str()+response.length());
			} else {
				response = F(":1,Out of memory during call to 'returnCString'");
			}
			return response.c_str();
			break;
		}       
		case 1:	// char* testCString(...) 
		{
			char* cstring;
			pc=strtok(NULL,',');
			if(NULL!=pc)
				cstring = unescapeString(pc);
			else {
				response=F(":1, param: 'cstring' missing!");
				return response.c_str();
			}

			char* ret_testCString = testCString(cstring);
			response = F(":0,");
			if(response.reserve(response.length()+escapeString(ret_testCString,NULL)+1)){
				escapeString(ret_testCString, response.c_str()+response.length());
			} else {
				response = F(":1,Out of memory during call to 'testCString'");
			}
			return response.c_str();
			break;
		}       
		case 2:	// double testDouble(...) 
		{
			double par;
			pc=strtok(NULL,',');
			if(NULL!=pc)
				par = atof( pc );
			else {
				response=F(":1, param: 'par' missing!");
				return response.c_str();
			}
            response = F(":0,");
			dtostre(testDouble( par ), response.c_str()+response.length(),DBL_PREC,0);
			return response.c_str();
			break;
		}       
		case 3:	// float testFloat(...) 
		{
			float par;
			pc=strtok(NULL,',');
			if(NULL!=pc)
				par = atof( pc );
			else {
				response=F(":1, param: 'par' missing!");
				return response.c_str();
			}
            response = F(":0,");
			dtostre(testFloat( par ), response.c_str()+response.length(),DBL_PREC,0);
			return response.c_str();
			break;
		}       
		case 4:	// int8_t testChar(...) 
		{
			int8_t cin;
			pc=strtok(NULL,',');
			if(NULL!=pc)
				cin = atoi( pc );
			else {
				response=F(":1, param: 'cin' missing!");
				return response.c_str();
			}
            response = F(":0,");
			response += String( testChar( cin ) );
			return response.c_str();
			break;
		}       
		case 5:	// int16_t testInt16(...) 
		{
			int16_t par;
			pc=strtok(NULL,',');
			if(NULL!=pc)
				par = atoi( pc );
			else {
				response=F(":1, param: 'par' missing!");
				return response.c_str();
			}
            response = F(":0,");
			response += String( testInt16( par ) );
			return response.c_str();
			break;
		}       
		case 6:	// int32_t testInt32(...) 
		{
			int32_t par;
			pc=strtok(NULL,',');
			if(NULL!=pc)
				par = atol( pc );
			else {
				response=F(":1, param: 'par' missing!");
				return response.c_str();
			}
            response = F(":0,");
			response += String( testInt32( par ) );
			return response.c_str();
			break;
		}       
		case 7:	// uint8_t testUChar(...) 
		{
			uint8_t cin;
			pc=strtok(NULL,',');
			if(NULL!=pc)
				cin = atoi( pc );
			else {
				response=F(":1, param: 'cin' missing!");
				return response.c_str();
			}
            response = F(":0,");
			response += String( testUChar( cin ) );
			return response.c_str();
			break;
		}       
		case 8:	// uint16_t testUInt16(...) 
		{
			uint16_t par;
			pc=strtok(NULL,',');
			if(NULL!=pc)
				par = atoi( pc );
			else {
				response=F(":1, param: 'par' missing!");
				return response.c_str();
			}
            response = F(":0,");
			response += String( testUInt16( par ) );
			return response.c_str();
			break;
		}       
		case 9:	// uint32_t testUInt32(...) 
		{
			uint32_t par;
			pc=strtok(NULL,',');
			if(NULL!=pc)
				par = atol( pc );
			else {
				response=F(":1, param: 'par' missing!");
				return response.c_str();
			}
            response = F(":0,");
			response += String( testUInt32( par ) );
			return response.c_str();
			break;
		}       
		case 10:	// void testVoid(...) 
		{
            testVoid();
			response = F(":0,");

			return response.c_str();
			break;
		}       
		default:
			response = F(":1, func_id: '");
			response += String(id);
			response += F("' invalid!");
			return response.c_str();
			break;
	}
}

