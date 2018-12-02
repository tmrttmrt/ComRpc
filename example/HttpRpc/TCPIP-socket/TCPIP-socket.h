#ifndef TCPIP_socket_h
#define TCPIP_socket_h

#define MAX_PARA_LEN 64
#define TIME_FOR_WIFI_CONFIG 10*60*1000
#define AP_SSID "Rpc-Server"
#define AP_PWD ""
const byte DNS_PORT = 53;

struct settings {
	char ssid[MAX_PARA_LEN] = "some-ssid";
	char password[MAX_PARA_LEN] = "some-passwd";
	short socket_port;
	unsigned long wifiTimeout=20000;
};

void save_settings();

#endif