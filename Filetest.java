import java.io.FileInputStream;
import java.io.FileOutputStream;

public class Filetest {

	public static void main(String[] args) {
		FileInputStream fis1=null;
		FileInputStream fis2=null;
		FileOutputStream fos=null;
		int a;
		try {
			fos=new FileOutputStream("pronlm40.txt");
			fis2=new FileInputStream("Prblm21.java");
			fis1=new FileInputStream("‪‪Prblm25.java");
			for(;(a=fis2.read())!=-1;)
				fos.write((char)a);
			for(;(a=fis1.read())!=-1;)
				fos.write((char)a);
			fis1.close();
			fis2.close();
			fos.close();
			System.out.println("File merged");
		}
		catch(Exception e) {
			System.out.println(e);
		}
	}

}