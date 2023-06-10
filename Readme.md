## Large File Sorter
The input is a large text file, where each line is a Number. String
For example:
```
415. Apple
30432. Something something something
1. Apple
32. Cherry is the best
2. Banana is yellow
```
Both parts can be repeated within the file. You need to get another file as output, where all
the lines are sorted. Sorting criteria: String part is compared first, if it matches then
Number.
Those in the example above, it should be:
```
1. Apple
415. Apple
2. Banana is yellow
32. Cherry is the best
30432. Something something something
```
There are two programs:
1. A utility for creating a test file of a given size. The result of the work is a text file
of the type described above. There is a number of lines with the same String
part.
2. The actual sorter. An important point, the file can be very large, e.g. ~100Gb.