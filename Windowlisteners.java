package sCalculator;

import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;

public class Windowlisteners implements WindowListener{
	MyFrame f; 
	public Windowlisteners(MyFrame x){
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

	@Override
	public void windowClosed(WindowEvent e) {
		System.out.println("Window Closed---");
		
	}

	@Override
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
