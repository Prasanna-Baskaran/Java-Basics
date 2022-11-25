import java.io.*;
import java.sql.*;
 
public class Hello{
    public static void main(String[] args) throws Exception
    {
        String url
            = "jdbc:mysql://localhost:3306/"; 
        String username = "root"; 
        String password = "123";
        String query
            = "select *from students"; 
        Class.forName(
            "com.mysql.cj.jdbc.Driver"); 
        Connection con = DriverManager.getConnection(url, username, password);
        System.out.println(
            "Connection Established successfully");
        Statement st = con.createStatement();
        ResultSet rs
            = st.executeQuery(query); 
        rs.next();
        String name
            = rs.getString("name"); 
 
        System.out.println(name); 
        st.close();
        con.close(); 
        System.out.println("Connection Closed....");
    }
}