#include <stdio.h>
#include <vector>
#include <iostream>
#include <cstdlib>
#include <ctime>
#include "windows.h"

using namespace std;

LARGE_INTEGER startingTime, endingTime, ellapsedMicrosecond, frequency;

void printArray(vector<int> array);
vector<int> createZeroArray(int n);
vector<int> inputArray(int n);
vector<int> generateArray(int n);
void printMenu();

vector<int> sortBubble(vector<int> array);
vector<int> sortInsertion(vector<int> array);
vector<int> sortSelect(vector<int> array);

int main() {
    srand(time(0));
    int choosed = 1;
    while (choosed != 0)
    {
        printMenu();
        scanf("%d", &choosed);

        switch (choosed)
        {
        case 0:
            cout << "Exit";
            break;
        case 1:
            int size;
            printf("Input array size: ");
            if (scanf("%d", &size) != 1 || size <= 0)
            {
                cout << "Incorrect\n";
            }
            else
            {
                cout << "Input array with size: " << size << "\n";
                vector<int> array = inputArray(size);

                cout << "\nResult select:\n";
                printArray(sortSelect(array));
                cout << "\nResult bubble:\n";
                printArray(sortBubble(array));
                cout << "\nResult insertion:\n";
                printArray(sortInsertion(array));
            }
            break;
            
        case 2:
        {
            vector<int> array, answer;
            for (int i = 2; i < 503; i += 50)
            {
                array = generateArray(i);

                // Select
                QueryPerformanceFrequency(&frequency);
                QueryPerformanceCounter(&startingTime);

                for (int i = 0; i < 100; i++)
                {
                    answer = sortSelect(array);
                }

                QueryPerformanceCounter(&endingTime);
                ellapsedMicrosecond.QuadPart = endingTime.QuadPart - startingTime.QuadPart;
                ellapsedMicrosecond.QuadPart *= 1000000;
                ellapsedMicrosecond.QuadPart /= frequency.QuadPart;
                long double selectMS = ellapsedMicrosecond.QuadPart / 100;

                // Bubble
                QueryPerformanceFrequency(&frequency);
                QueryPerformanceCounter(&startingTime);

                for (int i = 0; i < 100; i++)
                {
                    answer = sortBubble(array);
                }
                QueryPerformanceCounter(&endingTime);
                ellapsedMicrosecond.QuadPart = endingTime.QuadPart - startingTime.QuadPart;
                ellapsedMicrosecond.QuadPart *= 1000000;
                ellapsedMicrosecond.QuadPart /= frequency.QuadPart;

                long double bubbleMS = ellapsedMicrosecond.QuadPart / 100;

                // Insertion
                QueryPerformanceFrequency(&frequency);
                QueryPerformanceCounter(&startingTime);

                for (int i = 0; i < 100; i++)
                {
                    answer = sortInsertion(array);
                }
                
                QueryPerformanceCounter(&endingTime);
                ellapsedMicrosecond.QuadPart = endingTime.QuadPart - startingTime.QuadPart;
                ellapsedMicrosecond.QuadPart *= 1000000;
                ellapsedMicrosecond.QuadPart /= frequency.QuadPart;

                long double insertionMS = ellapsedMicrosecond.QuadPart / 100;

                cout << "\n Size: " << i << "\nBubble: " << bubbleMS << "     Select: " << selectMS << "       Insertion: " << insertionMS << "\n";
            }
            break;
        }
        default:
            cout << "Cant find this point, try again\n";
            break;
        }
    }
}

void printArray(vector<int> array)
{
    for (int i = 0; i < array.size(); i++)
    {
        printf("%6d", array[i]);
    }
    printf("\n");
}

vector<int> sortSelect(vector<int> array)
{
    int size = array.size();
    int temp = 0;
    vector<int> result = array;

    for (int i = 0; i < size - 1; i++)
    {
        int select = i;
        for (int j = i + 1; j < size; j++)
        {
            if (result[j] < result[select])
            {
                select = j;
            }
        }
        temp = result[select];
        result[select] = result[i];
        result[i] = temp;
    }

    return result;
}

vector<int> sortBubble(vector<int> array)
{
    int size = array.size();
    int temp = 0;
    vector<int> result = array;

    for (int i = 0; i + 1 < size; i++)
    {
        for (int j = 0; j + 1 < size - i; j++)
        {
            if (result[j + 1] < result[j])
            {
                temp = result[j];
                result[j] = result[j + 1];
                result[j + 1] = temp;
            }
        }
    }

    return result;
}

vector<int> sortInsertion(vector<int> array)
{
    int size = array.size();
    int temp = 0;
    vector<int> result = array;

    for (int i = 1; i < size; i++)
    {
        int j = i;
        temp = result[i];
        while (j > 0 && temp < result[j - 1])
        {
            result[j] = result[j - 1];
            j--;
        }
        result[j] = temp;
    }

    return result;
}

vector<int> createZeroArray(int size)
{
    vector<int> array;
    if (size > 0)
    {   
        array.reserve(size);
        for (int i = 0; i < size; i++)
        {
            array.push_back(0);
        }
    }

    return array;
}

vector<int> inputArray(int size)
{
    vector<int> array = createZeroArray(size);
    for (int i = 0; i < size; i++)
    {
        scanf("%d", &(array[i]));
    }

    return array;
}

void printMenu()
{
    cout << "\nMENU:\n0) Exit\n1) Demonstration\n2) Tests result\n";
}

vector<int> generateArray(int size)
{
    vector<int> array;
    for (int i = 0; i < size; i++)
    {
        array.push_back(rand());
    }
    return array;
}