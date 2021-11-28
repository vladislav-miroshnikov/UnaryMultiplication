# UnaryMultiplication

A tool for checking multiplications in unary system using Type-0 and Type-1 grammars.

### Step 1
To run application use from folder with `UnaryMultiplication.exe` file:

    .\UnaryMultiplication.exe T0
    
Or

    .\UnaryMultiplication.exe T1

where T0 - Type-0 grammar, T1 - Type-1 grammar.

### Step 2
On the command line, use `check [unary_expr]` to check for multiplication.

Example:

    check 11*1111=11111111
    True
    
    check 11*111=11111111
    False
    
To exit from application, use:

    exit
    
Or

    ctrl+z
