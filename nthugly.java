import java.util.*;
public class nthugly{ 
public static int maxdiv(int a,int b){
while(a%b==0)
a=a/b;
return a;
}
public static int isugly(int n){
n=maxdiv(n,2);
n=maxdiv(n,3);
n=maxdiv(n,5);
return (n==1)?1:0;
}
public static int nthugly(int n){
int i=1;
int count=1;
while(n>count){
i++;
if(isugly(i)==1)
count++;
}
return i;
}
public static void main(String[]args){
Scanner sc=new Scanner(System.in);
int n=sc.nextInt();
while(n!=0){
System.out.println(nthugly(n));
n=sc.nextInt();
 }
}
}