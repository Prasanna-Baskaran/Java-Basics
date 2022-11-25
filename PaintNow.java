package PaintTool;
import java.awt.*;
import java.awt.event.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

import javax.imageio.ImageIO;
import javax.swing.JColorChooser;
import javax.swing.JComboBox;
import javax.swing.JFileChooser;









class Windowlisteners implements WindowListener{
	Paint f; 
	public Windowlisteners(Paint x){
		f=x;
	}
	@Override
	public void windowOpened(WindowEvent e) {
		System.out.println("Window Opened---");
		
	}
	@Override
	public void windowClosing(WindowEvent e) {
		System.out.println("Closing....");
		f.dispose();
		
	}
	public void windowClosed(WindowEvent e) {
		System.out.println("Window Closed---");
		
	}
	public void windowIconified(WindowEvent e) {
		System.out.println("Window Retrieved---");
		
	}
	@Override
	public void windowDeiconified(WindowEvent e) {
		System.out.println("Window Minimized---");
		
	}
	@Override
	public void windowActivated(WindowEvent e) {
		System.out.println("Window Activated---");
		
	}
	@Override
	public void windowDeactivated(WindowEvent e) {
		System.out.println("Window Changed---");
	}
}

class MyActionListeners   implements ActionListener{
	Paint f;
	String str,sr;
	int z=0;
	Graphics g;
	MouseListeners c;
	MouseMotionListener s;
	JComboBox<?> j;
	MouseEvent e;
		public MyActionListeners(Paint x) {
			f=x;
		   g=x.g;
		   c=x.m;
		   s=x.ml;
		}
	
	public void actionPerformed(ActionEvent e) {
		str=e.getActionCommand();
		f.fp.setLocation(0, 0);
		f.sp.setLocation(0, 0);
		System.out.println(str);
		g=f.getGraphics();
		
	switch(str) {
		case "Colors":
			f.s=JColorChooser.showDialog(f, "Choose",Color.CYAN); 
			break;
		case"New..":
			g.setColor(Color.white);
			g.fillRect(0, 0, f.getSize().width, f.getSize().height);
		    f.repaint();
			break;
		case"Circle":
			g.setColor(f.s);
	              g.fillOval(f.fp.x,f.fp.y,f.sp.x,f.sp.y);
	              break;
		case"Rectangle":
			break;
		case"Square":
			break;
		case"Oval":
			break;
		
	case"3":
		f.size=3;
		break;
	case"5":
		f.size=5;
		break;
	case"8":
		f.size=8;
		break;
	case"10":
		f.size=10;
		break;
	case"12":
		f.size=12;
		break;
	case"14":
		f.size=14;
		break;
	case"16":
		f.size=16;
		break;
	case"18":
		f.size=18;
		break;
	case"20":
		f.size=20;
		break;
	case"24":
		f.size=24;
		break;
	case"28":
		f.size=28;
		break;
	case"32":
		f.size=32;
		break;
	case"64":
		f.size=64;
		break;
	case"Eraser":
		   f.s=Color.white; 
		break;
	case"Save":
		doSaveImage();
		break;
	}
	}
	
	  private void doSaveImage() {
	      JFileChooser fileChooser = new JFileChooser();
	      fileChooser.setFileSelectionMode(JFileChooser.FILES_ONLY);
	      int result = fileChooser.showSaveDialog(fileChooser);
	      if (result != JFileChooser.APPROVE_OPTION)
	         return;
	      int[][]r=null;
	      File saveFile = fileChooser.getSelectedFile();
	      if (!saveFile.getAbsolutePath().toLowerCase().endsWith(".jpg"))
	         saveFile = new File(saveFile.getAbsolutePath() + ".jpg");
	      BufferedImage image = new BufferedImage(f.getSize().width, f.getSize().height,BufferedImage.TYPE_INT_RGB);
	     
	      try {
	         ImageIO.write(image, "jpg", saveFile);
	      } catch (IOException e) {
	         return;
	      }
	   }
	  public boolean isPixel(Point blackPixel) {
	      return f.contains(blackPixel);
	   }
}












class MouseListeners implements MouseListener{

Paint f;
Graphics g;
public MouseListeners(Paint x) {
	f=x;
}
	public void mouseClicked(MouseEvent e) {
		f.fp.setLocation(0, 0);
		f.sp.setLocation(0, 0);
		f.fp.setLocation(e.getX(),e.getY());
	}

	@Override
	public void mousePressed(MouseEvent e) {
		f.fp.setLocation(0, 0);
		f.sp.setLocation(0, 0);
		f.fp.setLocation(e.getX(),e.getY());
	}

	@Override
	public void mouseReleased(MouseEvent e) {
		g=f.getGraphics();
		f.sp.setLocation(e.getX(),e.getY());
		if(f.a.str=="Line") {
			g.setColor(f.s);
	        g.drawLine(f.fp.x,f.fp.y,f.sp.x,f.sp.y);
			}
		
		if(f.a.str=="Circle") {
		g.setColor(f.s);
        g.drawOval(f.fp.x,f.fp.y,f.sp.y,f.sp.y);
		}
		if(f.a.str=="Rectangle") {
			int w=f.sp.x-f.fp.x;
			int h=f.sp.y-f.fp.y;
			g.setColor(f.s);
	        g.drawRect(f.fp.x,f.fp.y,w,h);
			}
		if(f.a.str=="Square") {
			g.setColor(f.s);
			int w=f.sp.x-f.fp.x;
	        g.drawRect(f.fp.x,f.fp.y,w,w);
			}
		if(f.a.str=="Oval") {
			g.setColor(f.s);
	        g.drawOval(f.fp.x,f.fp.y,f.sp.x,f.sp.y);
			}
	}

	@Override
	public void mouseEntered(MouseEvent e) {
		
	}

	@Override
	public void mouseExited(MouseEvent e) {
		
	}
	
}










class MouseMotionListeners implements MouseMotionListener{
Paint f;
Graphics g ;
public MouseMotionListeners(Paint x) {
	f=x;
}

	public void mouseDragged(MouseEvent e) {
	       	g =f.getGraphics();
	        g.setColor(f.s);
	        if(f.size<=5) {
	        g.fillRect(e.getX(), e.getY(),f. size, f.size);
	       }
	        else
	        	 g.fillOval(e.getX(), e.getY(),f. size, f.size);
		
	}

	@Override
	public void mouseMoved(MouseEvent e) {
	
	
		 }
}












 class Paint extends Frame{
Color s;
int size=5;
Frame f; 
Button b;
MenuBar m1;
JComboBox<Object> cb;
Menu v1,v2,v3,v4,v5,v6;
MenuItem mt1,mt2,mt3,mt4,mt5,mt6,mt7,mt8,mt9,mt10,cb1,cb2,cb3,cb4,cb5,cb6,cb7,cb8,cb9,cb10,cb11,cb12,p1,p2;
Windowlisteners x;
MyActionListeners a;
MouseListeners m;
Graphics g;
MouseMotionListeners ml;
Point fp=new Point(0,0);
Point sp=new Point(0,0);
    Paint(){
    
    	m1=new MenuBar();
    	v1=new Menu("File");
    	v2=new Menu("Colors");
    	v3=new Menu("Brush Size");
    	v6=new Menu("Brush Type");
    	v4=new Menu("Eraser");
    	v5=new Menu("Shape");
    	m1.add(v1);
    	m1.add(v2);
    	m1.add(v3);
    	m1.add(v4);
    	m1.add(v5);
    	mt1=new MenuItem("New..");
    	mt2=new MenuItem("Save");
    	v1.add(mt1);
    	v1.add(mt2);
    	mt3=new MenuItem("Colors");
    	v2.add(mt3);
    	mt4=new MenuItem("3");
    	cb1=new MenuItem("5");
    	cb2=new MenuItem("8");
    	cb3=new MenuItem("10");
    	cb4=new MenuItem("12");
    	cb5=new MenuItem("14");
    	cb6=new MenuItem("16");
    	cb7=new MenuItem("18");
    	cb8=new MenuItem("20");
    	cb9=new MenuItem("24");
    	cb10=new MenuItem("28");
    	cb11=new MenuItem("32");
    	cb12=new MenuItem("64");
    	p1=new MenuItem("Pencil");
    	p2=new MenuItem("Pen");
    	v6.add(p1);
    	v6.add(p2);
    	v3.add(mt4);
    	v3.add(cb1);
    	v3.add(cb2);
    	v3.add(cb3);
    	v3.add(cb4);
    	v3.add(cb5);
    	v3.add(cb6);
    	v3.add(cb7);
    	v3.add(cb8);
    	v3.add(cb9);
    	v3.add(cb10);
    	v3.add(cb11);
    	v3.add(cb12);
    	mt5=new MenuItem("Eraser");
    	v4.add(mt5);
    	mt6=new MenuItem("Circle");
		mt7=new MenuItem("Rectangle");
		mt8=new MenuItem("Square");
		mt9=new MenuItem("Oval");
		mt10=new MenuItem("Line");
		v5.add(mt10);
    	v5.add(mt6);
    	v5.add(mt7);
    	v5.add(mt8);
    	v5.add(mt9);
  
    		m=new MouseListeners(this);
    		addMouseListener(m);
    		ml=new MouseMotionListeners(this);
    		addMouseMotionListener(ml);
       		setSize(400, 400); 
       		a=new MyActionListeners(this);
       	 mt1.addActionListener(a);
       	 mt2.addActionListener(a);
       	 mt3.addActionListener(a);
       	 mt4.addActionListener(a);
       	 mt5.addActionListener(a);
       	 mt6.addActionListener(a);
       	 mt7.addActionListener(a);
       	 mt8.addActionListener(a);
       	 mt9.addActionListener(a);
       	 mt10.addActionListener(a);
       	 cb1.addActionListener(a);
       	 cb1.addActionListener(a);
       	 cb2.addActionListener(a);
       	 cb3.addActionListener(a);
       	 cb4.addActionListener(a);
       	 cb5.addActionListener(a);
       	 cb6.addActionListener(a);
       	 cb7.addActionListener(a);
       	 cb8.addActionListener(a);
       	 cb9.addActionListener(a);
       	 cb10.addActionListener(a);
       	 cb11.addActionListener(a);
       	 cb12.addActionListener(a);
       	 p1.addActionListener(a);
       	 p2.addActionListener(a);
       		setMenuBar(m1);
       		x=new Windowlisteners(this);
       		addWindowListener(x);
       		
        	setLayout(null);
        	setVisible(true);
        	
    }
    
}
 
 
 
 
 
 
 
 
 
public class PaintNow{
	public static void main(String []args) {
		Paint c=new Paint();
	}
}




