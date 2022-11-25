import java.util.*;
public class Fibonacci {
  public  static int fib(int n)
    {
        if (n <= 1)
            return n;
        return fib(n - 1) + fib(n - 2);
    }
public static void main(String[]args){
Scanner sc=new Scanner(System.in);
int n=sc.nextInt();
while(n!=0){
System.out.println(fib(n));
n=sc.nextInt();
 }
}
}