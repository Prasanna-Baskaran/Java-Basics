import java.util.Scanner;
public class Taxibooking
{
    public static void main(String args[])
    {
        Scanner s=new Scanner(System.in);
        Taxi[] taxi=new Taxi[10];
        int taxiId=4,bookingId=0;
        Booking[] booking=new Booking[20];
        taxi[0]=new Taxi();
        taxi[1]=new Taxi();
        taxi[2]=new Taxi();
        taxi[3]=new Taxi();
        int choice,availability;
        while(true)
        {
            System.out.println("__________Call_Taxi_Booking___________");
            System.out.println("1.Booking");
            System.out.println("2.Booking Details");
            System.out.println("3.Taxi Details");
            System.out.println("4.Exit");
            System.out.println("Enter ur choice:");
            choice=s.nextInt();
            switch(choice)
            {
                case 1:
                    availability=addBooking(booking,bookingId,taxi,taxiId);
                    if(availability!=-1)
                    {
                        System.out.println("Your booking is successfull with taxi no:"+(availability));
                        booking[bookingId].dropTime();
                        calcEarnings(booking,bookingId,taxi,availability,booking[bookingId].pickUpPoint,booking[bookingId].dropPoint);
                        bookingId++;
                    }
                    else
                    {
                        System.out.println("No taxi is free for your pickUpTime!!");
                        System.out.println("You may chance your pickup time and try your booking!!");
                    }
                    break;
                case 2:
                    System.out.println("BookingId    CustomerId    TaxiNo    PickupPoint    DropPoint   PickupTime    DropTime   Earnings");
                    for(int bookingid=0;bookingid<bookingId;bookingid++)
                    {
                        System.out.format("%-13d%-15d%-13d%-13s%-13s%-13d%-13d%-13d",bookingid,booking[bookingid].customer_id,booking[bookingid].taxiNo,booking[bookingid].pickUpPoint,booking[bookingid].dropPoint,booking[bookingid].pickUpTime,booking[bookingid].dropTime,booking[bookingid].earning);
                        System.out.println("");
                    }
                    break;
                case 3:
                    for(int taxiid=0;taxiid<taxiId;taxiid++)
                    {
                        int temp=0;
                        System.out.println("-----------------------------------");
                        System.out.format("%-10s%-10d%-10s%-10s","TaxiNo:",taxiid,"CurrentPosition: ",taxi[taxiid].currentPosition);
                        System.out.println("");
                        System.out.println("___________________________________");
                        System.out.println("-----------------------------------");
                        for(int bookingid=0;bookingid<bookingId;bookingid++)
                        {
                            if(booking[bookingid].taxiNo==taxiid)
                            {
                                if(temp==0)
                                    System.out.println("BookingId    CustomerId    TaxiNo    PickupPoint    DropPoint   PickupTime    DropTime   Earnings");
                                System.out.format("%-13d%-15d%-13d%-13s%-13s%-13d%-13d%-13d",bookingid,booking[bookingid].customer_id,booking[bookingid].taxiNo,booking[bookingid].pickUpPoint,booking[bookingid].dropPoint,booking[bookingid].pickUpTime,booking[bookingid].dropTime,booking[bookingid].earning);
                                System.out.println("");
                                temp=1;
                            }
                        }
                        System.out.format("%-10s%-10d","TotalEarnings:",taxi[taxiid].earning);
                        System.out.println("");
                    }
                    break;
                case 4:
                    return;
                default:
                    System.out.println("Please enter valid option!!!!");
            }
        }
    }
    public static int addBooking(Booking[] booking,int bookingId,Taxi[] taxi,int taxiId)
    {
        int availability,customer_id,pickUpTime;
        char pickUpPoint,dropPoint;
        Scanner s=new Scanner(System.in);
        System.out.println("Enter Customer id:");
        customer_id=s.nextInt();
        System.out.println("Enter Pickup point(a-f):");
        s.nextLine();
        pickUpPoint=s.nextLine().charAt(0);
        System.out.println("Enter Drop point(a-f):");
        dropPoint=s.nextLine().charAt(0);
        System.out.println("Enter pickup Time:");
        pickUpTime=s.nextInt();
        booking[bookingId]=new Booking(customer_id,pickUpPoint,dropPoint,pickUpTime);
        availability = booking[bookingId].checkAvailability(taxi,taxiId);
        return availability;
    }
    public static void calcEarnings(Booking[] booking,int bookingId,Taxi[] taxi,int taxiId,int pickUpPoint,int dropPoint)
    {
        int earning;
        earning=(((Math.abs(pickUpPoint-dropPoint)*15)-5)*10)+100;
        booking[bookingId].earning=earning;
        taxi[taxiId].earning+=earning;
    }
}
class Taxi
{
    char currentPosition;
    int earning,departureTime;
    public Taxi()
    {
        currentPosition='a';
    }
    public boolean isTaxiFree(int pickupTime)
    {
        if(departureTime<pickupTime)
            return false;
        return true;
    }
    public void departureTime(char pickUpPoint,char dropPoint,int pickUpTime)
    {
        departureTime=pickUpTime+Math.abs(pickUpPoint-dropPoint);
    }
} 
class Booking
{
    int customer_id,pickUpTime,dropTime,taxiNo,earning;
    char pickUpPoint,dropPoint;
    public Booking(int cust_id,char pickupPoint,char droppoint,int pickupTime)
    {
        customer_id=cust_id;
        pickUpPoint=pickupPoint;
        dropPoint=droppoint;
        pickUpTime=pickupTime;
    }
    public void dropTime()
    {
        this.dropTime=pickUpTime+Math.abs(pickUpPoint-dropPoint);
    }
    public int checkAvailability(Taxi[] taxi,int taxiCount)
    {
        int taxiId,taxiid=0,shortestDistance=6;
        for(taxiId=0;taxiId<taxiCount;taxiId++)
        {
            if(taxi[taxiId].isTaxiFree(pickUpTime))
            {
                if(Math.abs(taxi[taxiId].currentPosition-pickUpPoint)<shortestDistance)
                {
                    taxiid=taxiId;
                }
                if(Math.abs(taxi[taxiId].currentPosition-pickUpPoint)==shortestDistance)
                {
                    if(taxi[taxiId].earning<taxi[taxiid].earning)
                        taxiid=taxiId;
                }
            }
            shortestDistance=Math.abs(taxi[taxiid].currentPosition-pickUpPoint);
        }
        if(shortestDistance!=6)
        {
            taxi[taxiid].departureTime(pickUpPoint,dropPoint,pickUpTime);
            taxi[taxiid].currentPosition=dropPoint;
            taxiNo=taxiid;
            return taxiNo;
        }
        return -1;
    }
}