using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleData
{
    public class BinaryTreeOrder
    {
        static public void TreeOrdering(Int32[] arr)
        {
            if (arr.Length <= 1 )
            {
                Console.WriteLine("array length is less than 1, no need to order.");
                return;
            }

            Int32 len = arr.Length;
            for (Int32 i = len/2-1; i>=0; i--)
            {
                if (i * 2 +2 < len && arr[i * 2 + 2] > arr[i])
                {
                    Swaps(arr, i * 2 + 2, i);
                    MaintainBuild(arr, i * 2 + 2, len);
                }

                if ( arr[i * 2 + 1] > arr[i])
                {
                    Swaps(arr, i * 2 + 1, i);
                    MaintainBuild(arr, i * 2 + 1, len);
                }
            }

            for (Int32 i = 0; i < arr.Length - 1; ++i)
            {
                Swaps(arr, len - 1, 0);
                --len;

                if (len == 1)
                {
                    break;
                }
                MaintainBuild(arr, 0, len);
            }
        } 

        private static void Swaps(Int32[] arr, Int32 index1, Int32 index2)
        {
            Int32 tmp = arr[index1];
            arr[index1] = arr[index2];
            arr[index2] = tmp;
        }

        static public void MaintainBuild(Int32[] arr, Int32 index, Int32 len)
        {
            if (index > len / 2 )
            {
                return;
            }

            if (index * 2 + 1 < len)
            {
                if (arr[index] < arr[index*2 + 1])
                {
                    Swaps(arr, index * 2 + 1, index);

                    MaintainBuild(arr, index * 2 + 1, len);
                }

            }

            if (index * 2 + 2 < len)
            {
                if (arr[index] < arr[index * 2 + 2])
                {
                    Swaps(arr, index * 2 + 2, index);

                    MaintainBuild(arr, index * 2 + 2, len);
                }
            }

        }

        static public void Ordering(Int32[] arr)
        {
            if (arr.Length <= 1)
            {
                Console.WriteLine("array length is less than 1, no need to order.");
                return;
            }

            for (int i = arr.Length-1; i >0 ; i--)
            {
                for (int j = 0; j < i; j ++)
                {
                    if (arr[j+1] < arr[j])
                    {
                        int tmp = arr[j+1];
                        arr[j+1] = arr[j];
                        arr[j] = tmp;
                    }
                }
            }

            for (int i = arr.Length - 1; i >= 0; i--)
            {
                Console.Write(arr[i] + ", ");
            }
            Console.WriteLine();
        }
    }
}
