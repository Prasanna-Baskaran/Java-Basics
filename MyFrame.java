package sCalculator;

import java.awt.Button;
import java.awt.Color;
import java.awt.Frame;
import java.awt.GridLayout;
import java.awt.Label;
import java.awt.Panel;

import javax.swing.BoxLayout;

@SuppressWarnings("serial")
class MyFrame extends Frame {
	Label l1;
	Panel p1,p2,p3;
	Button b0,b1,b2,b3,b4,b5,b6,b7,b8,b9,b10,b11,b12,b13,b14,b15,b16,Sqr,Pi,bsp,fact,Ac,sq,xsqrty,plusmin,C,sinb,cosb,tanb,logb,History,memre,mpl,mmin,mstr,memc,e;
	Windowlisteners x;
	MyActionListeners m;
	public MyFrame(String t) {
		super(t);
		setSize(600,500);
		setBackground(Color.lightGray);
		l1=new Label("");
		l1.setBackground(Color.GRAY);
		l1.setBounds(50,50, 350, 45);
		add(l1);
		p1=new Panel();
		p1.setBounds(50, 100, 400, 300);
		p1.setBackground(Color.lightGray);
		add(p1);
		
		memc=new Button("MC");
		memc.setBounds(50, 100, 50, 40);
		p1.add(memc);
		
		memre=new Button("MR");
		memre.setBounds(50, 100, 50, 40);
		p1.add(memre);
		
		mpl=new Button("M+");
		mpl.setBounds(50, 100, 50, 40);
		p1.add(mpl);

		mmin=new Button("M-");
		mmin.setBounds(50, 100, 50, 40);
		p1.add(mmin);
		
		mstr=new Button("MS");
		mstr.setBounds(50, 100, 50, 40);
		p1.add(mstr);
		
		b1=new Button("1");
		b1.setBounds(50, 100, 50, 40);
		p1.add(b1);

		b2=new Button("2");
		b2.setBounds(50, 100, 50, 40);
		p1.add(b2);
		
		b3=new Button("3");
		b3.setBounds(50, 100, 50, 40);
		p1.add(b3);
		
		C=new Button("C");
		C.setBounds(50, 100, 50, 40);
		p1.add(C);
		
		Ac=new Button("AC");
	    Ac.setBounds(50, 100, 50,40);
		p1.add(Ac);
		
		b4=new Button("4");
		b4.setBounds(50, 100, 50, 40);
		p1.add(b4);
		
		b5=new Button("5");
		b5.setBounds(50, 100, 50, 40);
		p1.add(b5);
		
		b6=new Button("6");
		b6.setBounds(50, 100, 50, 40);
		p1.add(b6);
		
		sinb=new Button("Sin()");
		sinb.setBounds(50, 100, 50, 40);
		p1.add(sinb);
		
		cosb=new Button("Cos()");
		sinb.setBounds(50, 100, 50, 40);
		p1.add(cosb);
		
		b7=new Button("7");
		b7.setBounds(50, 100, 50, 40);
		p1.add(b7);
		
		b8=new Button("8");
		b8.setBounds(50, 100, 50, 40);
		p1.add(b8);
		
		b9=new Button("9");
		b9.setBounds(50, 100, 50, 40);
		p1.add(b9);
		
		tanb=new Button("Tan()");
		tanb.setBounds(50, 100, 50, 40);
		p1.add(tanb);
		
		logb=new Button("Log()");
		sinb.setBounds(50, 100, 50, 40);
		p1.add(logb);
		
		b0=new Button(".");
		b0.setBounds(50, 100, 50, 40);
		p1.add(b0);
		
		b10=new Button("0");
		b10.setBounds(50, 100, 50, 40);
		p1.add(b10);
		
		bsp=new Button("<-X");
		bsp.setBounds(50, 100, 50, 40);
		p1.add(bsp);
	
		plusmin=new Button("[+/-]");
		plusmin.setBounds(50, 100, 50,40);
		p1.add(plusmin);
		
		fact=new Button("!n");
		fact.setBounds(50, 100, 50, 40);
		p1.add(fact);
		
		Sqr=new Button("Sqrt()");
		Sqr.setBounds(50, 100, 50, 40);
		p1.add(Sqr);
		
		Pi=new Button("Pi");
		Pi.setBounds(50, 100, 50, 40);
		p1.add(Pi);
		
		sq=new Button("x^2");
		sq.setBounds(50,100,50,40);
		p1.add(sq);
		
		xsqrty=new Button("X^y");
		xsqrty.setBounds(50, 100, 50, 40);
		p1.add(xsqrty);
		
		
		b11=new Button("=");
		b11.setBounds(50, 100, 50, 40);
		p1.add(b11);
		
		b12=new Button("+");
		b12.setBounds(50, 100, 50, 40);
	
		
		b13=new Button("-");
		b13.setBounds(50, 100, 50, 40);
	


		b14=new Button("*");
		b14.setBounds(50, 100, 50, 40);
	
		
		b15=new Button("/");
		b15.setBounds(50, 100, 50, 40);
		
		
		b16=new Button("%");
		b16.setBounds(50, 100, 50, 40);
	     History=new Button("History");
	     History.setBounds(50, 100, 75, 50);
		
		p2=new Panel();
		p2.setBounds(470, 125, 100, 250);
		add(p2);
		p2.add(b12);
		p2.add(b13);
		p2.add(b14);
		p2.add(b15);
		p2.add(b16);
		
		p3=new Panel();
		p3.setBounds(50, 425, 100,50 );
		add(p3);
		p3.add(History);
		m=new MyActionListeners(this);
		b0.addActionListener(m);
		b1.addActionListener(m);
		b2.addActionListener(m);
		b3.addActionListener(m);
		b4.addActionListener(m);
		b5.addActionListener(m);
		b6.addActionListener(m);
		b7.addActionListener(m);
		b8.addActionListener(m);
		b9.addActionListener(m);
		b10.addActionListener(m);
		b11.addActionListener(m);
		b12.addActionListener(m);
		b13.addActionListener(m);
		b14.addActionListener(m);
		b15.addActionListener(m);
		b16.addActionListener(m);
		Sqr.addActionListener(m);
		Pi.addActionListener(m);
		bsp.addActionListener(m);
		fact.addActionListener(m);
		Ac.addActionListener(m);
		sq.addActionListener(m);
		xsqrty.addActionListener(m);
		plusmin.addActionListener(m);
		C.addActionListener(m);
		sinb.addActionListener(m);
		cosb.addActionListener(m);
		tanb.addActionListener(m);
		logb.addActionListener(m);
		memre.addActionListener(m);
		mpl.addActionListener(m);
		mmin.addActionListener(m);
		mstr.addActionListener(m);
		memc.addActionListener(m);
		History.addActionListener(m);
		x=new Windowlisteners(this);
		addWindowListener(x);
		p1.setLayout(new GridLayout(6,5,7,7));
		p2.setLayout(new BoxLayout (p2, BoxLayout.Y_AXIS));
		p3.setLayout(new BoxLayout (p3, BoxLayout.X_AXIS) );
		this.setLayout(null);
		this.setVisible(true);
		this.setResizable(false);
		
	}

}

