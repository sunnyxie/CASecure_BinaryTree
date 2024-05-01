# CASecure_BinaryTree

command = new Oracle.DataAccess.Client.OracleCommand(), when I did the copy, forget to add the
parameter, which is the pkg Procedure Name.

def swap_new(arr, index1, index2):
  """Swaps two elements in a list using tuple unpacking.
  Args:
      arr: The list to swap elements in.
      index1: The index of the first element to swap.
      index2: The index of the second element to swap.
  """
  arr[index1], arr[index2] = arr[index2], arr[index1]


def array_sort(arr):
    max_len = len(arr)
    array_adjust(arr, 0, max_len)
    print (arr, ' --- ')

    for i in range(0, max_len-1):
        swap_new(arr, 0, max_len - i - 1)
        array_adjust(arr, 0, max_len - i - 1)  # here 2nd pass as max_len. 
    swap_new(arr, 0, 1)


def build_heap(arr, start_index, max_len):
    max_index = int(max_len / 2 - 1)
    for i in range(max_index, start_index - 1 , -1):
        largest = i
        if arr[largest] < arr[i*2+1]:
            largest= i * 2 + 1
            #array_adjust(arr, i, max_len)
            
        if i*2+2 < max_len and arr[largest] < arr[i*2+2]:
            largest= i * 2 + 2

        if largest != i:
            swap_new(arr, largest, i)
            build_heap(arr, i, max_len)


if __name__ == "__main__":
    array1 = [9, 6, 10, 2, 3, 5, 31, 21, 8, 15, 7, 0]
    swap_new(array1, 0, 2 )
    
    array_sort(array1)
