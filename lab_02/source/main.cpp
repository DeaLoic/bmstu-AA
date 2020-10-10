#include <stdio.h>
#include <vector>
#include <iostream>
#include <cstdlib>
#include <ctime>
#include "windows.h"

using namespace std;

LARGE_INTEGER startingTime, endingTime, ellapsedMicrosecond, frequency;

void printMatrix(vector< vector<int>> matrix);
vector<vector<int>> createZeroMatrix(int n, int m);
vector<vector<int>> inputMatrix(int n, int m);
vector< vector<int> > generateMatrix(int n, int m);
void printMenu();

vector<vector<int>> multiplyVinograd(vector< vector<int>> first, vector< vector<int>> second);
vector<vector<int>> multiplyClassic(vector< vector<int>> first, vector< vector<int>> second);
vector<vector<int>> multiplyVinogradOpt(vector< vector<int>> first, vector< vector<int>> second);

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
            int nFirst, mFirst;
            printf("Input first matrix size: ");
            if (scanf("%d %d", &nFirst, &mFirst) != 2 || nFirst <= 0 || mFirst <= 0)
            {
                cout << "Incorrect\n";
            }
            else
            {
                cout << "Input first matrix with size " << nFirst << "x" << mFirst << "\n";
                vector< vector<int> > matrixFirst = inputMatrix(nFirst, mFirst);

                int nSecond, mSecond;
                cout << "Input second matrix size: ";
                if (scanf("%d %d", &nSecond, &mSecond) != 2 || nFirst <= 0 || mFirst <= 0)
                {
                    cout << "Incorrect\n";
                }
                else if (nSecond != mFirst)
                {
                    cout << "Incorrect size. Matrix cant be multiply\n";
                }
                else
                {
                    cout << "Input second matrix with size " << nSecond << "x" << mSecond << "\n";
                    vector< vector<int> > matrixSecond = inputMatrix(nSecond, mSecond);

                    cout << "\nResult classic:\n";
                    printMatrix(multiplyClassic(matrixFirst, matrixSecond));
                    cout << "\nResult Vinograd:\n";
                    printMatrix(multiplyVinograd(matrixFirst, matrixSecond));
                    cout << "\nResult Vinograd optimized:\n";
                    printMatrix(multiplyVinogradOpt(matrixFirst, matrixSecond));
                }
            }
            
        case 2:
        {
            vector< vector<int> > matrixFirst, matrixSecond, answer;
            for (int i = 2; i < 503; i += 50)
            {
                matrixFirst = generateMatrix(i, i);
                matrixSecond = generateMatrix(i, i);

                // Classic
                QueryPerformanceFrequency(&frequency);
                QueryPerformanceCounter(&startingTime);

                answer = multiplyClassic(matrixFirst, matrixSecond);

                QueryPerformanceCounter(&endingTime);
                ellapsedMicrosecond.QuadPart = endingTime.QuadPart - startingTime.QuadPart;
                ellapsedMicrosecond.QuadPart *= 1000000;
                ellapsedMicrosecond.QuadPart /= frequency.QuadPart;
                long double classicMS = ellapsedMicrosecond.QuadPart;

                // VINOGRAD
                QueryPerformanceFrequency(&frequency);
                QueryPerformanceCounter(&startingTime);

                answer = multiplyVinograd(matrixFirst, matrixSecond);
                
                QueryPerformanceCounter(&endingTime);
                ellapsedMicrosecond.QuadPart = endingTime.QuadPart - startingTime.QuadPart;
                ellapsedMicrosecond.QuadPart *= 1000000;
                ellapsedMicrosecond.QuadPart /= frequency.QuadPart;

                long double vinogradMS = ellapsedMicrosecond.QuadPart;

                // OPT
                QueryPerformanceFrequency(&frequency);
                QueryPerformanceCounter(&startingTime);

                answer = multiplyVinogradOpt(matrixFirst, matrixSecond);
                
                QueryPerformanceCounter(&endingTime);
                ellapsedMicrosecond.QuadPart = endingTime.QuadPart - startingTime.QuadPart;
                ellapsedMicrosecond.QuadPart *= 1000000;
                ellapsedMicrosecond.QuadPart /= frequency.QuadPart;

                long double vinogradOptMS = ellapsedMicrosecond.QuadPart;

                cout << "\n Size: " << i << "\nClassic: " << classicMS << "     Vinograd: " << vinogradMS << "       Opt: " << vinogradOptMS << "\n";
            }
            break;
        }
        default:
            cout << "Cant find this point, try again\n";
        }
    }
}

void printMatrix(vector< vector<int>> matrix)
{
    for (int i = 0; i < matrix.size(); i++)
    {
        for (int j = 0; j < matrix[i].size(); j++)
        {
            printf("%6d", matrix[i][j]);
        }
        printf("\n");
    }
}

vector<vector<int>> multiplyClassic(vector< vector<int>> first, vector< vector<int>> second)
{
    int nFirst = first.size();
    int nSecond = second.size();
   
    vector<vector<int>> result;
    //printf("%d %d %d %d", nFirst, nSecond, first[0].size(), second[0].size());
    if (nFirst > 0 && nSecond > 0 && first[0].size() == nSecond && second[0].size() != 0)
    {
        int mSecond = second[0].size();

        result = createZeroMatrix(nFirst, mSecond);
        for (int i = 0; i < nFirst; i++)
        {
            for (int j = 0; j < mSecond; j++)
            {
                for (int k = 0; k < nSecond; k++)
                {
                    result[i][j] += first[i][k] * second[k][j];
                }
            }
        }
    }

    //printMatrix(result);
    return result;
}

vector<vector<int>> multiplyVinograd(vector< vector<int>> first, vector< vector<int>> second)
{
    int nFirst = first.size();
    int nSecond = second.size();
   
    vector<vector<int>> result;

    if (nFirst > 0 && nSecond > 0 && first[0].size() == nSecond && second[0].size() != 0)
    {
        int mSecond = second[0].size();
        int mFirst = nSecond;

        result = createZeroMatrix(nFirst, mSecond);

        vector<int> mulH, mulV;
        mulH.reserve(nFirst);
        mulV.reserve(mSecond);

        for (int i = 0; i < nFirst; i++)
        {
            mulH[i] = 0;
            for (int j = 0; j < mFirst / 2; j++)
            {
                mulH[i] += first[i][j * 2] * first[i][j * 2 + 1];
            }
        }

        for (int i = 0; i < mSecond; i++)
        {
            mulV[i] = 0;
            for (int j = 0; j < nSecond / 2; j++)
            {
                mulV[i] += second[j * 2][i] * second[j * 2 + 1][i];
            }
        }

        for (int i = 0; i < nFirst; i++)
        {
            for (int j = 0; j < mSecond; j++)
            {
                result[i][j] = - mulH[i] - mulV[j];
                for (int k = 0; k < mFirst / 2; k++)
                {
                    result[i][j] += (first[i][2 * k + 1] + second[2 * k][j]) * (first[i][2 * k] + second[2 * k + 1][j]);
                }
            }
        }

        if (mFirst % 2 == 1)
        {
            for (int i = 0; i < nFirst; i++)
            {
                for (int j = 0; j < mSecond; j++)
                {
                    result[i][j] += first[i][mFirst - 1] * second[mFirst - 1][j];
                }
            }
        }
    }

    return result;
}

vector<vector<int>> multiplyVinogradOpt(vector< vector<int>> first, vector< vector<int>> second)
{
    int nFirst = first.size();
    int nSecond = second.size();
   
    vector<vector<int>> result;

    if (nFirst > 0 && nSecond > 0 && first[0].size() == nSecond && second[0].size() != 0)
    {
        int mSecond = second[0].size();
        int mFirst = nSecond;

        result = createZeroMatrix(nFirst, mSecond);

        vector<int> mulH, mulV;
        mulH.reserve(nFirst);
        mulV.reserve(mSecond);

        int m1Mod2 = mFirst % 2;
        int n2Mod2 = nSecond % 2;

        int temp = nSecond - n2Mod2;
        for (int i = 0; i < mSecond; i++)
        {
            mulV[i] = 0;
            for (int j = 0; j < temp; j += 2)
            {
                mulV[i] += second[j][i] * second[j + 1][i];
            }
        }

        temp = mFirst - m1Mod2;
        for (int i = 0; i < nFirst; i++)
        {
            mulH[i] = 0;
            for (int j = 0; j < temp; j += 2)
            {
                mulH[i] += first[i][j] * first[i][j + 1];
            }
        }

        for (int i = 0; i < nFirst; i++)
        {
            for (int j = 0; j < mSecond; j++)
            {
                int buff = - (mulH[i] + mulV[j]);
                for (int k = 0; k < temp; k += 2)
                {
                    buff += (first[i][k + 1] + second[k][j]) * (first[i][k] + second[k + 1][j]);
                }
                result[i][j] = buff;
            }
        }

        if (m1Mod2 == 1)
        {
            temp = mFirst - 1;
            for (int i = 0; i < nFirst; i++)
            {
                for (int j = 0; j < mSecond; j++)
                {
                    result[i][j] += first[i][temp] * second[temp][j];
                }
            }
        }
    }

    return result;
}

vector<vector<int>> createZeroMatrix(int n, int m)
{
    vector< vector<int>> matrix;
    if (n > 0 && m > 0)
    {   matrix.reserve(n);
        for (int i = 0; i < n; i++) {
            matrix.push_back(vector<int> {});
            matrix[i].reserve(m);
            for (int j = 0; j < m; j++)
            {
                matrix[i].push_back(0);
            }
        }
    }

    return matrix;
}

vector<vector<int>> inputMatrix(int n, int m)
{
    vector<vector<int>> matrix = createZeroMatrix(n, m);
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < m; j++)
        {
            scanf("%d", &(matrix[i][j]));
        }
    }

    return matrix;
}

void printMenu()
{
    cout << "\nMENU:\n0) Exit\n1) Demonstration\n2) Tests result\n";
}

vector< vector<int> > generateMatrix(int n, int m)
{
    vector< vector<int> > matrix;
    for (int i = 0; i < n; i++)
    {
        matrix.push_back(vector<int> {});

        for (int j = 0; j < m; j++)
        {
            matrix[i].push_back(rand());
        }
    }
    return matrix;
}