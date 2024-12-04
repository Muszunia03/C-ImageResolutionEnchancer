Assembler Code


section .data
    bmp_file db "image.bmp", 0     
    output_msg db "Image width: ", 0
    output_height db "Image height: ", 0
    error_msg db "Error loading file!", 0
    width dd 0                        
    height dd 0                 
    buffer rb 54               

section .bss
    file_descriptor resb 4         

section .text
    extern printf, fopen, fread, fclose
    global _start

_start:
    ; Otwórz plik
    mov rdi, bmp_file               
    call fopen
    test rax, rax                
    jz error_exit
    mov [file_descriptor], rax

    ; Wczytaj nagłówek BMP
    mov rdi, [file_descriptor]      
    mov rsi, buffer             
    mov rdx, 54                
    call fread

    ; Pobierz szerokość (bajty 18-21)
    mov eax, dword [buffer + 18]  
    mov [width], eax

    ; Pobierz wysokość (bajty 22-25)
    mov eax, dword [buffer + 22] 
    mov [height], eax

    ; Wyświetl szerokość i wysokość
    mov rdi, output_msg
    call printf
    mov rdi, [width]
    call printf

    mov rdi, output_height
    call printf
    mov rdi, [height]
    call printf

    ; Zamknij plik
    mov rdi, [file_descriptor]
    call fclose

    ; Wyjście
    mov rax, 60
    xor rdi, rdi
    syscall

error_exit:
    mov rdi, error_msg
    call printf
    mov rax, 60
    mov rdi, 1
    syscall
