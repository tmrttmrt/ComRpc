#ifndef DIYRPC_H
#define DIYRPC_H

#define RET_BUFF_SIZE 64
#define DBL_PREC 15

size_t escapeString(const char * str, char * estr);
char* unescapeString(char * estr);
const char* rpc_proc_line(char *line);
uint32_t atouint32_t(char *);

#endif
