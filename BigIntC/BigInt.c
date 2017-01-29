#include "BigInt.h"

bigInt* newBigInt(int *toBigInt, int neg, int length)
{
    if(length < 0)
    {
        return NULL;
    }
    bigInt *toReturn = (bigInt*)calloc(1,sizeof(bigInt));
    toReturn -> length = length;
    toReturn -> neg  = neg == 1 ? 1 : 0;

    if(length == 0)
    {
        toReturn -> neg = 0;
        toReturn -> length = 1;
        toReturn -> arr = (int*)calloc(1,sizeof(int));
        toReturn -> arr[0] = 0;
        return toReturn;
    }

    toReturn -> arr = (int*)calloc((unsigned int)length, sizeof(int));
    int x;
    for(x = 0; x < length; x++)
    {
        toReturn -> arr[x] = toBigInt[x];
    }

    int *test = (int*)calloc(1, sizeof(int));
    test[0] = 0;
    bigInt *bigTest = (bigInt*)calloc(1,sizeof(bigInt));
    bigTest -> arr = test;
    bigTest -> length = 1;
    if(comp(toReturn, bigTest) == 0)
    {
        toReturn -> neg = 0;
    }

    free(bigTest -> arr);
    free(bigTest);

    return toReturn;
}

bigInt* copy(bigInt *toCopy)
{
    bigInt *copy = (bigInt*)calloc(1, sizeof(bigInt));

    copy -> arr = copyArray(toCopy -> arr, toCopy -> length);
    copy -> length = toCopy -> length;
    copy -> neg = toCopy -> neg;

    return copy;
}

int* copyArray(int *toCopy, int length)
{
    int *copy = (int*)calloc((unsigned int)length, sizeof(int));

    int x = 0;
    for(; x < length; x++)
    {
        copy[x] = toCopy[x];
    }

    return copy;
}

bigInt* newBigIntFromNum(int num)
{
    bigInt *toReturn = (bigInt*)calloc(1, sizeof(bigInt));
    toReturn -> neg = (num < 0 ? 1 : 0);
    int temp = num;
    int count = 0;
    while(temp > 0)
    {
        temp = temp/10;
        count++;
    }
    if(count == 0)
    {
        count = 1;
    }

    toReturn -> length = count;

    int *array = (int*)calloc((unsigned int)count, sizeof(int));

    int x;
    for(x = count - 1; x > -1; x--)
    {
        array[x] = num%10;
        num = num/10;
    }
    toReturn -> arr = array;

    return toReturn;
}

bigInt* add(bigInt *first, bigInt *second)
{
    if(first -> neg != second -> neg)
    {
        if(first -> neg)
        {
            return subArray(second, first);
        }
        else
        {
            return subArray(first, second);
        }
    }
    int length = (comp(first, second) == -1 ? second -> length : first -> length) + 1;
    int temp[length];

    int i;
    for(i = 0; i < length; i++)
    {
        temp[i] = 0;
    }

    int x, y, z, carry = 0;
    for(x = first -> length - 1, y = second -> length - 1, z = length -1; x > -1 || y > -1; y--, x--, z-- )
    {
        temp[z] = (x > -1 ? (first -> arr)[x] : 0) + (y > -1 ? (second -> arr)[y] : 0) + carry;
        carry = 0;
        if(temp[z] > 9)
        {
            temp[z] = temp[z] % 10;
            carry = 1;
        }
    }
    temp[z] += carry;
    bigInt *toReturn = (bigInt*)calloc(1,sizeof(bigInt));
    toReturn -> neg = first -> neg == 1 ? 1 : 0;
    fixArray(toReturn, temp, length);
    return toReturn;
}

void fixArray(bigInt* toReturn, int *toFix, int length)
{
    int index = -1, x;
    for(x = 0; x < length && index == -1; x++)
    {
        if(toFix[x] != 0)
        {
            index = x;
        }
    }

    if(index == -1)
    {
        toReturn -> length = 1;
        int *final = (int*)calloc(1, sizeof(int));
        final[0] = 0;
        toReturn -> arr = final;
        toReturn -> neg = 0;
    }
    else
    {
        toReturn->length = length - index;
        int *final = (int*) calloc((unsigned int)toReturn->length, sizeof(int));
        for (x = 0; index < length; index++, x++)
        {
            final[x] = toFix[index];
        }
        toReturn->arr = final;
    }
}

bigInt* sub(bigInt *first, bigInt *second)
{
    bigInt *temp = (bigInt*)calloc(1, sizeof(bigInt));
    temp -> length = second -> length;
    temp -> neg = (second -> neg == 1 ? 0 : 1);
    temp -> arr = (int*)calloc((unsigned int)temp -> length, sizeof(int));

    int x;
    for(x = 0; x < temp -> length; x++)
    {
        (temp -> arr)[x] = (second -> arr)[x];
    }

    bigInt *retNum = add(first, temp);
    freeBigInt(temp);
    return retNum;
}

bigInt* subArray(bigInt *first, bigInt *second)
{
    bigInt *toReturn = (bigInt*)calloc(1, sizeof(bigInt));
    int check = comp(first, second);
    toReturn -> neg = check == -1 ? 1 : 0;
    int length = first -> length > second -> length ? first -> length : second -> length;
    int *temp = (int*)calloc((unsigned int)length, sizeof(int));
    int *copy = check == -1 ? second -> arr : first -> arr;

    int x;
    for(x = 0; x < length; x++)
    {
        temp[x] = copy[x];
    }

    bigInt *smaller = check == -1 ? first : second;

    int y, a;
    int index;
    for(x = length -1, y = smaller -> length -1; y > -1; y--, x--)
    {
        temp[x] = temp[x] - (smaller -> arr)[y];
        if(temp[x] < 0)
        {
            temp[x] += 10;
            index = -1;
            for(a = x-1; a > -1 && index == -1; a--)
            {
                if(temp[a] > 0)
                {
                    index = a;
                    temp[a]--;
                }
            }
            for(a = x-1; a > index; a--)
            {
                temp[a] = 9;
            }
        }
    }

    fixArray(toReturn, temp, length);
    free(temp);
    return toReturn;
}

bigInt* multiply(bigInt *first, bigInt *second)
{
    bigInt *final = newBigIntFromNum(0);
    bigInt *temp;
    bigInt *transition;

    int x, y, outerScale = 0, innerScale;
    for(x = first -> length -1; x > -1; x--)
    {
        innerScale = 0;
        for(y = second -> length -1; y > -1; y--)
        {
            temp = newBigIntFromNum((first -> arr)[x] * (second -> arr)[y]);
            shift(temp, outerScale + innerScale);
            transition = add(final, temp);
            freeBigInt(temp);
            freeBigInt(final);
            final = transition;
            innerScale++;
        }
        outerScale++;
    }

    if(first -> neg == second -> neg)
    {
        final -> neg = 0;
    }
    else
    {
        final -> neg = 1;
    }
    return final;
}

bigInt* divide(bigInt *first, bigInt *second)
{
    bigInt *count = newBigIntFromNum(0);
    if(comp(second, count) == 0)
    {
        free(count);
        return NULL;
    }
    bigInt *one = newBigIntFromNum(1);
    bigInt *temp;

    bigInt *transition = copy(first);
    transition -> neg = 0;
    bigInt *secondCopy = copy(second);
    secondCopy -> neg = 0;

    while(comp(transition, second) > -1)
    {
        temp = sub(transition, secondCopy);
        freeBigInt(transition);
        transition = temp;
        temp = add(count, one);
        freeBigInt(count);
        count = temp;
    }

    freeBigInt(transition);
    freeBigInt(secondCopy);
    freeBigInt(one);

    if(first -> neg != second -> neg)
    {
        count -> neg = 1;
    }

    return count;
}

void shift(bigInt* val, int amountToShift)
{
    int *shifted = (int*)calloc((unsigned int)val -> length + amountToShift, sizeof(int));

    int x;
    for(x = 0; x < val -> length; x++)
    {
        shifted[x] = (val -> arr)[x];
    }
    for(; x < val -> length + amountToShift; x++)
    {
        shifted[x] = 0;
    }
    val -> length = val -> length + amountToShift;
    free(val -> arr);
    val -> arr = NULL;
    val -> arr = shifted;
}

int compBig(bigInt *first, bigInt *second)
{
    int compare = comp(first, second);

    if(compare == 0 && first -> neg != second -> neg)
    {
        return first -> neg == 1 ? -1 : 1;
    }
    else if(compare == -1)
    {
        return second -> neg == 1 ? 1 : -1;
    }
    else if(compare == 1)
    {
        return first -> neg == 1 ? -1 : 1;
    }
    else
    {
        return compare;
    }
}

int comp(bigInt *first, bigInt *second)
{
    if(first -> length < second -> length)
    {
        return -1;
    }
    else if(first -> length > second -> length)
    {
        return 1;
    }
    int x;
    for(x = 0; x < first -> length; x++)
    {
        if((first -> arr)[x] > (second -> arr)[x])
        {
            return 1;
        }
        else if((first -> arr)[x] < (second -> arr)[x])
        {
            return -1;
        }
    }
    return 0;
}

void printBig(bigInt *val, FILE *fout)
{
    if(fout != NULL)
    {
        int x;
        if (val->neg) {
            fprintf(fout, "-");
        }
        for (x = 0; x < val->length; x++) {
            fprintf(fout, "%d", (val->arr)[x]);
        }
        fprintf(fout, "\n");
    }
}

void freeBigInt(bigInt *val)
{
    free(val -> arr);
    val -> arr = NULL;
    free(val);
}
