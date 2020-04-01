using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace ConsoleApp2
{
    class Program
    {
        static string bitArrayToStr(BitArray bitArray)
        {
            string result = "";

            for (int i = bitArray.Count - 1; i >= 0; i--)
            {
                result += (bitArray[i]) ? 1 : 0;

                if ((i) % 8 == 0)
                    result += " ";
            }

            return result;
        }

        static void shiftLeft(ref BitArray arr)
        {
            for (int i = arr.Count - 1; i > 0; i--)
            {
                arr[i] = arr[i - 1];
            }

            arr[0] = false;
        }

        static void shiftRight(ref BitArray arr)
        {
            arr[0] = false;
            for (int i = 1; i < arr.Count; i++)
            {
                arr[i - 1] = arr[i];
            }
        }

        static BitArray twoComp(BitArray arr)
        {
            BitArray result = new BitArray(arr.Count);
            for (int i = 0; i < arr.Count; i++)
            {
                result[i] = !arr[i];
            }

            BitArray mem = new BitArray(arr.Count);
            mem[0] = true;

            result = addBinary(result, mem);
            return result;
        }
        static BitArray addBinary(BitArray add1, BitArray add2)
        {
            BitArray result = new BitArray(add1.Count);

            bool mem = false;

            for (int i = 0; i < add1.Count; i++)
            {
                if (((add1[i] && !add2[i]) || (!add1[i] && add2[i])) && !mem)
                {
                    result[i] = true;
                    continue;
                }

                if (((add1[i] && !add2[i]) || (!add1[i] && add2[i])) && mem)
                {
                    result[i] = false;
                    continue;
                }

                if (add1[i] && add2[i] && !mem)
                {
                    mem = true;
                    result[i] = false;
                    continue;
                }
                if (add1[i] && add2[i] && mem)
                {
                    mem = true;
                    result[i] = true;
                    continue;
                }

                if (!add1[i] && !add2[i] && !mem)
                {
                    result[i] = false;
                    continue;
                }

                if (!add1[i] && !add2[i] && mem)
                {
                    result[i] = true;
                    mem = false;
                }

            }

            return result;
        }

        static BitArray substruct(BitArray decresing, BitArray subtrahend)
        {
            BitArray result = addBinary(decresing, twoComp(subtrahend));
            return result;
        }

        static BitArray getLeftPart(BitArray arr)
        {
            BitArray result = new BitArray(arr.Count / 2);
            for (int i = 0; i < arr.Count / 2; i++)
            {
                result[i] = arr[i + arr.Count / 2];
            }

            return result;
        }

        static BitArray getRightPart(BitArray arr)
        {
            BitArray result = new BitArray(arr.Count / 2);
            for (int i = 0; i < arr.Count / 2; i++)
            {
                result[i] = arr[i];
            }
            return result;
        }

        static BitArray concatenate(BitArray arr1, BitArray arr2)
        {
            BitArray result = new BitArray(arr1.Count * 2);
            for (int i = 0; i < arr1.Count; i++)
            {
                result[i] = arr1[i];
            }

            for (int i = 0; i < arr2.Count; i++)
            {
                result[i + arr2.Count] = arr2[i];
            }

            return result;
        }

        static int getIntFromBitArray(BitArray bitArray)
        {

            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];

        }

        static BitArray divide(BitArray divident, BitArray divisor)
        {
            BitArray result = new BitArray(COUNT_BITS);

            BitArray reminder = new BitArray(COUNT_BITS * 2);

            for (int i = 0; i < COUNT_BITS; i++)
            {
                reminder[i] = divident[i];
            }

            Console.WriteLine($"Reminder\n{bitArrayToStr(reminder)}");

            shiftLeft(ref reminder);

            Console.WriteLine($"Shift Left\n{bitArrayToStr(reminder)}");

            BitArray tmp;

            for (int j = 0; j < COUNT_BITS; j++)
            {
                /*
                 * Substruct divisor from left part of register
                 * and place result to left part or register
                 */

                tmp = getLeftPart(reminder);
                tmp = substruct(tmp, divisor);
                reminder = concatenate(getRightPart(reminder), tmp);
                Console.WriteLine($"Reminder=reminder-divisor\n{bitArrayToStr(reminder)}");

                /*
                 * If reminder > 0 (MSD==1) then shift left
                 * and set LSB=1
                 */
                if (!reminder[reminder.Count - 1])
                {
                    Console.WriteLine("reminder>0, shift left, set lsb = 1");
                    shiftLeft(ref reminder);
                    reminder[0] = true;
                    Console.WriteLine(bitArrayToStr(reminder));
                }
                else
                {
                    /*
                     * If reminder < 0 add divisor to left part of reminder
                     * shift the reminder left and set rightmost bit to 0
                     */
                    tmp = getLeftPart(reminder);
                    tmp = addBinary(tmp, divisor);
                    reminder = concatenate(getRightPart(reminder), tmp);
                    shiftLeft(ref reminder);
                    reminder[0] = false;
                    Console.WriteLine($"Reminder=reminder+divisor,shift left, r[0]=0\n{bitArrayToStr(reminder)}");
                }
            }

            result = getLeftPart(reminder);
            shiftRight(ref result);

            Console.WriteLine($"Quotient: {getIntFromBitArray(getRightPart(reminder))}\t reminder: {getIntFromBitArray(result)}");

            return result;
        }

        private static int COUNT_BITS;
        static void Main(string[] args)
        {

            Console.WriteLine("write count of bits:");
            COUNT_BITS = int.Parse(Console.ReadLine());

            BitArray dividend = new BitArray(COUNT_BITS);
            BitArray divisor = new BitArray(COUNT_BITS);

            int div = int.Parse(Console.ReadLine());
            dividend = new BitArray(new int[] { div });
            Console.WriteLine($"Divident:\n{bitArrayToStr(dividend)}");

            int divis = int.Parse(Console.ReadLine());
            divisor = new BitArray(new int[] { divis });
            Console.WriteLine($"Divisor\n{bitArrayToStr(divisor)}\n");

            BitArray quotient = divide(dividend, divisor);

        }
    }
}
