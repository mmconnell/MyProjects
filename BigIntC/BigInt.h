#ifndef SAFEC_BIGINT_H
#define SAFEC_BIGINT_H

#include "stdio.h"
#include "stdlib.h"
#include "string.h"
#include "unistd.h"

struct BIGINT
{
    int *arr;
    int neg;
    int length;
};
typedef struct BIGINT bigInt;

bigInt* newBigInt(int *, int, int);
bigInt* newBigIntFromNum(int);
bigInt* copy(bigInt*);
int* copyArray(int*, int);
bigInt* add(bigInt*, bigInt*);
bigInt* addArray(bigInt*, bigInt*);
bigInt* sub(bigInt*, bigInt*);
bigInt* subArray(bigInt*, bigInt*);
bigInt* multiply(bigInt*, bigInt*);
bigInt* divide(bigInt*, bigInt*);
bigInt* mod(bigInt*,bigInt*);
void shift(bigInt*,int);
void fixArray(bigInt*,int*,int);
int compBig(bigInt*,bigInt*);
int comp(bigInt*, bigInt*);
void printBig(bigInt*, FILE *);
void freeBigInt(bigInt*);
#endif
