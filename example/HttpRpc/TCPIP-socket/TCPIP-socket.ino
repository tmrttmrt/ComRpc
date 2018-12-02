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

#include <ESP8266WiFi.h>
#include <DNSServer.h>
#include <ESP8266WiFiMulti.h>
#include <ESP8266WebServer.h>
#include <EEPROM.h>
#include "webserver.h"
#include "ComRpc.h"
#include "TCPIP-socket.h"


#define AP_SSID "Rpc-Server"
#define AP_PWD ""
#define TIME_FOR_WIFI_CONFIG 10*60*1000


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


ESP8266WiFiMulti WiFiMulti;
WiFiUDP ntpUDP;
extern ESP8266WebServer server;
extern long last_page_load;
String esp_chipid;
struct settings currSettings;
WiFiServer* wifiServer;

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

void save_settings(){
	uint8_t * ptr;
	unsigned int sum;
	
	ptr=(uint8_t *)&currSettings;
	sum=0;
	for(int i=0;i<sizeof(settings);i++){
		sum+=ptr[i];
	}
	EEPROM.begin(sizeof(settings)+sizeof(sum));
	EEPROM.put(0,currSettings);
	EEPROM.put(sizeof(settings),sum);
	EEPROM.end();
}

void load_settings(){
	uint8_t * ptr;
	unsigned int sum;
	unsigned int EEPROM_sum;
	struct settings savedSettings;
	
	Serial.println("Loading settins from EEPROM ...");
	EEPROM.begin(sizeof(settings)+sizeof(sum));
	EEPROM.get(0,savedSettings);
	EEPROM.get(sizeof(settings),EEPROM_sum);
	ptr=(uint8_t *)	&savedSettings;
	sum=0;
	for(int i=0;i<sizeof(settings);i++){
		sum+=ptr[i];
	}
	if(sum==EEPROM_sum){
		currSettings=savedSettings;
	} else {
		Serial.println("");
		Serial.println(F("EEPROM checksum does not match!"));
		Serial.println(F("Using default settings."));
		//		currSettings=savedSettings;
	}
	EEPROM.end();
}


void ap_config(){
	char ap_ssid[33] = "";
	char ap_pwd[65] = "";
	DNSServer dnsServer;
	IPAddress apIP(192, 168, 4, 1);
	IPAddress netMsk(255, 255, 255, 0);
	
	Serial.println("");
	(String(AP_SSID) + "-" + esp_chipid).toCharArray(ap_ssid,sizeof(ap_ssid));
	strcpy(ap_pwd, AP_PWD);

	Serial.println(String( F("Setting configuration access point ")) + ap_ssid);
	Serial.println(String( F("Access point password: '")) + ap_ssid + "'");
	Serial.println(String( F("Configure at: 'http://")) + apIP.toString() + "/'");
	
	WiFi.softAPConfig(apIP, apIP, netMsk);
	WiFi.softAP(ap_ssid, ap_pwd);

	dnsServer.setErrorReplyCode(DNSReplyCode::NoError);
	dnsServer.start(DNS_PORT, "*", apIP);
	setup_webserver();

	while (((millis() - last_page_load) < TIME_FOR_WIFI_CONFIG)) {
		dnsServer.processNextRequest();
		server.handleClient();
	}

}

bool connect_WiFi(){
	// Connecting to WiFi network
	Serial.println();
	Serial.print("Connecting to ");
	Serial.println(currSettings.ssid);
	
	//WiFi.begin(ssid, password);
	WiFiMulti.addAP(currSettings.ssid, currSettings.password);
	unsigned long start_time=millis();
	while (WiFiMulti.run() != WL_CONNECTED) {
		delay(500);
		Serial.print(".");
		if(millis()-start_time > currSettings.wifiTimeout){
			Serial.println("");
			Serial.print("Timeout when connecting to ");
			Serial.println(currSettings.ssid);
			ap_config();
			return false;
		}
	}
	Serial.println("");
	Serial.println("WiFi connected");
	// Printing the ESP IP address
	Serial.println(WiFi.localIP());
	return true;
}

void setup() {
	// put your setup code here, to run once:
	Serial.begin(115200);

	load_settings();
	Serial.println("");
	Serial.println(F("Configure mode ..."));
	esp_chipid = String(ESP.getChipId());
	connect_WiFi();
	setup_webserver();
	Serial.println("HTTP server started");
	wifiServer=new WiFiServer(currSettings.socket_port);
	wifiServer->begin();

}


String line_string;

void loop() {
	
	WiFiClient client = wifiServer->available();
	server.handleClient();
	if (client) {
		Serial.println("Client connected");
		while (client.connected()) {
			while (client.available()>0) {
				char c = client.read();
				Serial.write(c);
				line_string.concat(c);
				if('\n'==c){
					line_string.trim();
					if(line_string.length()==0){
						break;
					}
					Serial.println(line_string);
					client.println(line_string);	
					if(line_string[0]==':'){ //ComRpc
						const char* ret=rpc_proc_line((char*) line_string.c_str());
						client.println(ret);
						Serial.println(ret);
						line_string="";
						break;
					} else {
						line_string="";
					}
				}
			}
			delay(10);
		}

		client.stop();
		Serial.println("Client disconnected");
	}
}
