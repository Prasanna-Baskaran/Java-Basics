package sCalculator;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

public class MyActionListeners implements ActionListener{
	MyFrame f;
	History h;
	String s="",input;
	String op="";
	

	float input1=0,input2=0,mem=0;
		public MyActionListeners(MyFrame x) {
			f=x;
		}
	public void actionPerformed(ActionEvent e) {
		s=e.getActionCommand();
		switch(s) {
		
		case "0":
			 f.l1.setText(f.l1.getText()+0);
			 break;
			 
		case "1":
			 f.l1.setText(f.l1.getText()+1);
			 break;
			 
		case "2":
			 f.l1.setText(f.l1.getText()+2);
			 break;
			 
		case "3":
			 f.l1.setText(f.l1.getText()+3);
			 break;
			 
		case "4":
			 f.l1.setText(f.l1.getText()+4);
			 break;
			 
		case "5":
			 f.l1.setText(f.l1.getText()+5);
			 break;
			 
		case "6":
			 f.l1.setText(f.l1.getText()+6);
			 break;
			 
		case "7":
			 f.l1.setText(f.l1.getText()+7);
			 break;
			 
		case "8":
			 f.l1.setText(f.l1.getText()+8);
			 break;
			 
		case "9":
			 f.l1.setText(f.l1.getText()+9);
			 break;
			 
		case ".":
			 input=f.l1.getText();
			 input+=".";
			 f.l1.setText(input);
			 break;
			 
		case "+":
			 input1=Float.parseFloat(f.l1.getText());
			    op="+";
			    f.l1.setText("");
			    break;
			    
		case "-":
			 input1=Float.parseFloat(f.l1.getText());
			    op="-";
			    f.l1.setText("");
			    break;
			    
		case "*":
			 input1=Float.parseFloat(f.l1.getText());
			    op="*";
			    f.l1.setText("");
			    break;
			    
		case "/":
			 input1=Float.parseFloat(f.l1.getText());
			    op="/";
			    f.l1.setText("");
			    break;
			    
		case "%":
			 input1=Float.parseFloat(f.l1.getText());
			    op="%";
			    f.l1.setText("");
			    break;
		case "M+":
			 mem+=Float.parseFloat(f.l1.getText());
			 f.l1.setText("");
			 op="m+";
			 break;
			 
		case "M-":
			 mem+=Float.parseFloat(f.l1.getText());
			 f.l1.setText("");
			 op="m-";
			 break;
			    
		case "MR":
			f.l1.setText(Float.toString(mem));
		  	break;
	    case "MC":
			mem=0;
			f.l1.setText("0");
		  	break;
			
		case "MS":
		  		input1=Float.parseFloat(f.l1.getText());
		  		mem=input1;
		  		f.l1.setText("");
		  	break;
		  	
		case "E":
			f.l1.setText(Double.toString(Math.E));
			break;
		  	
		case "C":
			  	f.l1.setText("");
			  	break;
			  
		case "AC":
			 	f.l1.setText("");
			    input1=0;
			    input2=0;
			    input="";
			    op="";
			    break;
		case "<-X":
			     input=f.l1.getText();
			     f.l1.setText(input.substring(0, input.length()-1));
			     break;
			    
		case "Sin()":
				input1=Float.parseFloat(f.l1.getText());
				f.l1.setText(Double.toString(Math.sin(input1)));
				break;
				
		case "Cos()":
				input1=Float.parseFloat(f.l1.getText());
				f.l1.setText(Double.toString(Math.cos(input1)));
				break;
			
		case "Tan()":
				input1=Float.parseFloat(f.l1.getText());
				f.l1.setText(Double.toString(Math.tan(input1)));
				break;
		
		case "Log()":
			input1=Float.parseFloat(f.l1.getText());
			f.l1.setText(Double.toString(Math.log(input1)));
			break;
			
		case "!n":
			input1=Float.parseFloat(f.l1.getText());
			f.l1.setText(Integer.toString(Fact(input1)));
			break;
			
		case "[+/-]":
		    input=f.l1.getText();
		     f.l1.setText(Integer.toString(Integer.parseInt(input)*-1));
		     break;
		     
		case "Sqrt()":
			input1=Float.parseFloat(f.l1.getText());
			f.l1.setText(Double.toString(Math.sqrt(input1)));
			break;
		
		case "Pi":
			f.l1.setText(Double.toString(Math.PI));
			break;

		case "x^2":
			input1=Float.parseFloat(f.l1.getText());
			f.l1.setText(Double.toString(Math.pow(input1,2)));
			break;
			    

		case "X^y":
			input1=Float.parseFloat(f.l1.getText());
			op="x^y";
			    f.l1.setText("");
			    break;
		case "History":
			       h.setSize(0, 0);
			       h.setVisible(true);
			
		     
		case "=":
			   input2=Float.parseFloat(f.l1.getText());
			   if(op=="+") 
		             	f.l1.setText(Float.toString(input1+input2));
			   else if(op=="-")      
		        	   	f.l1.setText(Float.toString(input1-input2));
			   else if(op=="*")
		        	   	f.l1.setText(Float.toString(input1*input2));
			   else if(op=="/")
				   		f.l1.setText(Float.toString(input1/input2));
		       else if(op=="%")
		    	   		f.l1.setText(Float.toString(input1%input2));   	   		
		       else if(op=="x^y")
		    	   f.l1.setText(Float.toString((float) Math.pow( input1,input2)));
		       else if(op=="m+")
		    	   f.l1.setText(Float.toString(input1+input2));
		       else if(op=="m-")
		    	   f.l1.setText(Float.toString(input1-input2));
		       else 
		    	   f.l1.setText("select operator");
			   	break;
		default :
			 input=f.l1.getText();
			 	f.l1.setText("Error");
			 	input=f.l1.getText();
			 	f.l1.setText(input);
		}
	}
	private int Fact(float input12) {
        int f = 1;
        for(int i = 1; i <= input12; ++i)
        {
            
            f*= i;
        }
		return f;
	}

}
