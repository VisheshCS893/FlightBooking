using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FlightBooking.Controllers
{
    public class TicketController : Controller
    {
        // GET: Ticket
        public ActionResult BookTicket(string name, int numberOfTickets)
        {
            List<Ticket> tickets = new List<Ticket>();
            if (numberOfTickets == 1)
            {
                tickets = BookOneTicket(name);
            }
            else if (numberOfTickets == 2)
            {
                tickets = BookTwoTickets(name);
            }
            else if (numberOfTickets == 3)
            {
                tickets = BookThreeTickets(name);
            }
            else
            {
                tickets = BookNTickets(name, numberOfTickets);
            }
            return View(tickets);
        }

        public ActionResult viewBookingDetails()
        {
            List<row_details> rowDetails;
            using (var context = new FlightbookingEntities())
            {
                rowDetails = context.row_details.ToList<row_details>();

            }


            return View(rowDetails);
        }

        public ActionResult viewTicketDetails(string rowNo, int seatNo)
        {
            List<Ticket> tickets = new List<Ticket>();
            using (var context = new FlightbookingEntities())
            {
                tickets = context.Ticket.Where(ticket => ticket.row_no.Equals(rowNo) && ticket.seat_no == seatNo).ToList<Ticket>();

            }

            return View("~/Views/Ticket/BookTicket.cshtml", tickets);
        }

        private List<Ticket> BookOneTicket(string name)
        {
            List<Ticket> tickets = new List<Ticket>();
            using (var context = new FlightbookingEntities())
            {
                List<row_details> rowDetails = context.row_details.ToList<row_details>();

                Ticket ticket = new Ticket();
                ticket.applicant_name = name;
                foreach (var row in rowDetails)
                {
                    List<string> seats;
                    ticket.row_no = row.row_no;
                    if (!String.IsNullOrEmpty(row.seats_filled))
                    {
                        seats = row.seats_filled.Split(',').ToList<string>();
                        if (seats.Count == 7)
                        {
                            continue;
                        }
                        else if (!seats.Contains("3"))
                        {
                            ticket.seat_no = 3;
                            row.seats_filled = row.seats_filled + ",3";
                            break;
                        }

                        else if (!seats.Contains("4"))
                        {
                            ticket.seat_no = 4;
                            row.seats_filled = row.seats_filled + ",4";
                            break;
                        }
                        else if (!seats.Contains("5"))
                        {
                            ticket.seat_no = 5;
                            row.seats_filled = row.seats_filled + ",5";
                            break;
                        }
                        else if (!seats.Contains("1"))
                        {
                            ticket.seat_no = 1;
                            row.seats_filled = row.seats_filled + ",1";

                            break;
                        }
                        else if (!seats.Contains("2"))
                        {

                            row.seats_filled = row.seats_filled + ",2";
                            ticket.seat_no = 2;
                            break;
                        }
                        else if (!seats.Contains("6"))
                        {

                            row.seats_filled = row.seats_filled + ",6";
                            ticket.seat_no = 6;
                            break;
                        }
                        else if (!seats.Contains("7"))
                        {

                            row.seats_filled = row.seats_filled + ",7";
                            ticket.seat_no = 7;
                            break;
                        }
                    }
                    else
                    {
                        ticket.seat_no = 3;
                        row.seats_filled = row.seats_filled + "3";
                        break;
                    }

                }

                context.Ticket.Add(ticket);

                context.SaveChanges();
                tickets.Add(ticket);

            }
            return tickets;
        }
        private List<Ticket> BookTwoTickets(string name)
        {
            List<Ticket> tickets = new List<Ticket>();
            using (var context = new FlightbookingEntities())
            {
                List<row_details> rowDetails = context.row_details.ToList<row_details>();

                Ticket ticket1 = new Ticket();
                Ticket ticket2 = new Ticket();
                ticket1.applicant_name = name;
                ticket2.applicant_name = name;
                bool isConsecutive = false;
                foreach (var row in rowDetails)
                {
                    List<string> seats;
                    if (!String.IsNullOrEmpty(row.seats_filled))
                    {
                        seats = row.seats_filled.Split(',').ToList<string>();
                        if (seats.Count == 7)
                        {
                            continue;
                        }
                        else if (!seats.Contains("1") && !seats.Contains("2"))
                        {
                            ticket1.seat_no = 1;
                            ticket2.seat_no = 2;
                            ticket1.row_no = row.row_no;
                            ticket2.row_no = row.row_no;
                            isConsecutive = true;
                            row.seats_filled = row.seats_filled + "," + 1 + "," + 2;
                            break;
                        }
                        else if (!seats.Contains("6") && !seats.Contains("7"))
                        {
                            ticket1.seat_no = 6;
                            ticket2.seat_no = 7;
                            ticket1.row_no = row.row_no;
                            ticket2.row_no = row.row_no;
                            isConsecutive = true;
                            row.seats_filled = row.seats_filled + "," + 6 + "," + 7;
                            break;
                        }

                    }
                    else
                    {
                        ticket1.seat_no = 1;
                        ticket2.seat_no = 2;
                        ticket1.row_no = row.row_no;
                        ticket2.row_no = row.row_no;
                        isConsecutive = true;
                        row.seats_filled = 1 + "," + 2;
                        break;
                    }





                }



                if (!isConsecutive)
                {
                    foreach (var row in rowDetails)
                    {
                        List<string> seats;

                        if (!String.IsNullOrEmpty(row.seats_filled))
                        {
                            seats = row.seats_filled.Split(',').ToList<string>();
                            if (seats.Count == 7)
                            {
                                continue;
                            }
                            else
                            {
                                for (int i = 2; i <= 6; i++)
                                {
                                    int j = i + 1;
                                    if (!seats.Contains(i.ToString()) && !seats.Contains(j.ToString()))
                                    {
                                        ticket1.seat_no = i;
                                        ticket2.seat_no = j;
                                        ticket1.row_no = row.row_no;
                                        ticket2.row_no = row.row_no;
                                        isConsecutive = true;
                                        row.seats_filled = row.seats_filled + "," + i + "," + j;

                                        break;
                                    }
                                }
                                if (isConsecutive)
                                {
                                    break;
                                }


                            }


                        }
                        else
                        {
                            ticket1.seat_no = 1;
                            ticket2.seat_no = 2;
                            ticket1.row_no = row.row_no;
                            ticket2.row_no = row.row_no;
                            isConsecutive = true;
                            row.seats_filled = 1 + "," + 2;

                            break;
                        }
                    }

                }



                if (!isConsecutive)
                {
                    bool isNextSeatOfPreviousRow = false;
                    foreach (var row in rowDetails)
                    {



                        List<string> seats = row.seats_filled.Split(',').ToList<string>();

                        if (seats.Count == 7)
                        {
                            continue;
                        }
                        else
                        if (isNextSeatOfPreviousRow)
                        {
                            isNextSeatOfPreviousRow = false;
                            for (int i = 1; i <= 7; i++)
                            {

                                if (!seats.Contains(i.ToString()))
                                {
                                    ticket2.seat_no = i;
                                    ticket2.row_no = row.row_no;
                                    row.seats_filled = row.seats_filled + "," + i;

                                    break;

                                }


                            }
                            break;
                        }

                        else
                        {
                            for (int i = 1; i <= 7; i++)
                            {

                                if (!seats.Contains(i.ToString()))
                                {
                                    if (seats.Count == 6)
                                    {
                                        isNextSeatOfPreviousRow = true;
                                        ticket1.seat_no = i;
                                        ticket1.row_no = row.row_no;
                                        row.seats_filled = row.seats_filled + "," + i;
                                        break;

                                    }
                                    else
                                    {
                                        for (int j = i + 2; j <= 7; j++)
                                        {
                                            if (!seats.Contains(j.ToString()))
                                            {

                                                ticket1.seat_no = i;
                                                ticket2.seat_no = j;
                                                ticket1.row_no = row.row_no;
                                                ticket2.row_no = row.row_no;
                                                row.seats_filled = row.seats_filled + "," + i + "," + j;

                                                break;

                                            }
                                        }
                                        break;
                                    }


                                }


                            }
                            if (!isNextSeatOfPreviousRow)
                            {
                                break;
                            }


                        }

                    }


                }
                context.Ticket.Add(ticket1);
                context.Ticket.Add(ticket2);
                context.SaveChanges();
                tickets.Add(ticket1);
                tickets.Add(ticket2);
            }

            return tickets;
        }
        private List<Ticket> BookThreeTickets(string name)
        {
            List<Ticket> tickets = new List<Ticket>();
            bool previousTwoRowsNextSeat = false;
            bool previousRowNextSeat = false;
            using (var context = new FlightbookingEntities())
            {

                List<row_details> rowDetails = context.row_details.ToList<row_details>();
                Ticket ticket1 = new Ticket();
                Ticket ticket2 = new Ticket();
                Ticket ticket3 = new Ticket();
                ticket1.applicant_name = name;
                ticket2.applicant_name = name;
                ticket3.applicant_name = name;
                bool isConsecutive = false;
                foreach (var row in rowDetails)
                {
                    List<string> seats;
                    if (!String.IsNullOrEmpty(row.seats_filled))
                    {
                        seats = row.seats_filled.Split(',').ToList<string>();
                        if (seats.Count == 7)
                        {
                            continue;
                        }
                        else if (!seats.Contains("3") && !seats.Contains("4") && !seats.Contains("5"))
                        {
                            ticket1.seat_no = 3;
                            ticket2.seat_no = 4;
                            ticket3.seat_no = 5;
                            ticket1.row_no = row.row_no;
                            ticket2.row_no = row.row_no;
                            ticket3.row_no = row.row_no;
                            isConsecutive = true;
                            row.seats_filled = row.seats_filled + "," + 3 + "," + 4 + "," + 5;
                            break;
                        }



                    }
                    else
                    {
                        ticket1.seat_no = 3;
                        ticket2.seat_no = 4;
                        ticket3.seat_no = 5;
                        ticket1.row_no = row.row_no;
                        ticket2.row_no = row.row_no;
                        ticket3.row_no = row.row_no;
                        isConsecutive = true;
                        row.seats_filled = 3 + "," + 4 + "," + 5;
                        break;

                    }


                }

                if (!isConsecutive)
                {
                    foreach (var row in rowDetails)
                    {
                        List<string> seats;

                        if (!String.IsNullOrEmpty(row.seats_filled))
                        {
                            seats = row.seats_filled.Split(',').ToList<string>();
                            if (seats.Count == 7)
                            {
                                continue;
                            }
                            else
                            {
                                for (int i = 1; i <= 5; i++)
                                {
                                    int j = i + 1;
                                    int k = i + 2;

                                    if (!seats.Contains(i.ToString()) && !seats.Contains(j.ToString()) && !seats.Contains(k.ToString()))
                                    {
                                        ticket1.seat_no = i;
                                        ticket2.seat_no = j;
                                        ticket3.seat_no = k;
                                        ticket1.row_no = row.row_no;
                                        ticket2.row_no = row.row_no;
                                        ticket3.row_no = row.row_no;
                                        isConsecutive = true;
                                        row.seats_filled = row.seats_filled + "," + i + "," + j + "," + k;

                                        break;
                                    }
                                }
                                if (isConsecutive)
                                {
                                    break;
                                }

                            }
                        }
                    }


                }

                if (!isConsecutive)
                {

                    int rowCountForNextSeat = 1;
                    foreach (var row in rowDetails)
                    {
                        List<string> seats = row.seats_filled.Split(',').ToList<string>();
                        if (seats.Count == 7)
                        {
                            continue;
                        }
                        else
                        if (previousRowNextSeat)
                        {
                            previousRowNextSeat = false;
                            for (int i = 1; i <= 7; i++)
                            {

                                if (!seats.Contains(i.ToString()))
                                {
                                    ticket3.seat_no = i;
                                    ticket3.row_no = row.row_no;
                                    row.seats_filled = row.seats_filled + "," + i;

                                    break;

                                }


                            }
                            break;

                        }
                        else

                        if (previousTwoRowsNextSeat)
                        {


                            if (seats.Count <= 5 && rowCountForNextSeat == 1)
                            {
                                for (int i = 1; i <= 7; i++)
                                {

                                    if (!seats.Contains(i.ToString()))
                                    {
                                        for (int j = i + 1; j <= 7; j++)
                                        {
                                            if (!seats.Contains(j.ToString()))
                                            {

                                                ticket2.row_no = row.row_no;
                                                ticket2.seat_no = i;
                                                ticket3.row_no = row.row_no;
                                                ticket3.seat_no = j;
                                                row.seats_filled = row.seats_filled + "," + i + "," + j;
                                                break;
                                            }
                                        }

                                    }
                                }
                                previousTwoRowsNextSeat = false;
                                break;

                            }
                            else
                            {

                                for (int i = 1; i <= 7; i++)
                                {
                                    if (!seats.Contains(i.ToString()))
                                    {
                                        if (rowCountForNextSeat == 1)
                                        {
                                            ticket2.row_no = row.row_no;
                                            ticket2.seat_no = i;
                                            row.seats_filled = row.seats_filled + "," + i;
                                            break;

                                        }
                                        else
                                        {
                                            ticket3.row_no = row.row_no;
                                            ticket3.seat_no = i;
                                            row.seats_filled = row.seats_filled + "," + i;
                                            previousTwoRowsNextSeat = false;
                                            break;
                                        }
                                    }
                                }




                            }

                            rowCountForNextSeat++;


                        }
                        else
                        {

                            for (int i = 1; i <= 7; i++)
                            {

                                if (!seats.Contains(i.ToString()))
                                {

                                    if (seats.Count == 6)
                                    {
                                        previousTwoRowsNextSeat = true;
                                        ticket1.row_no = row.row_no;
                                        ticket1.seat_no = i;
                                        row.seats_filled = row.seats_filled + "," + i;
                                        break;

                                    }

                                    for (int j = i + 1; j <= 7; j++)
                                    {


                                        if (!seats.Contains(j.ToString()))
                                        {

                                            if (seats.Count == 5)
                                            {
                                                previousRowNextSeat = true;
                                                ticket1.row_no = row.row_no;
                                                ticket1.seat_no = i;
                                                ticket2.row_no = row.row_no;
                                                ticket2.seat_no = j;
                                                row.seats_filled = row.seats_filled + "," + i + "," + j;
                                                break;

                                            }

                                            for (int k = j + 1; k <= 7; k++)
                                            {

                                                if (!seats.Contains(k.ToString()))
                                                {

                                                    ticket1.seat_no = i;
                                                    ticket2.seat_no = j;
                                                    ticket3.seat_no = k;
                                                    ticket1.row_no = row.row_no;
                                                    ticket2.row_no = row.row_no;
                                                    ticket3.row_no = row.row_no;
                                                    row.seats_filled = row.seats_filled + "," + i + "," + j + "," + k;

                                                    break;

                                                }


                                            }
                                        }

                                        break;
                                    }




                                }





                            }



                        }

                        if (!(previousTwoRowsNextSeat || previousRowNextSeat))
                        {
                            break;
                        }

                    }



                }
                context.Ticket.Add(ticket1);
                context.Ticket.Add(ticket2);
                context.Ticket.Add(ticket3);
                context.SaveChanges();
                tickets.Add(ticket1);
                tickets.Add(ticket2);
                tickets.Add(ticket3);

            }
            return tickets;
        }

        private List<Ticket> BookNTickets(string name, int numOfTickets)
        {
            List<Ticket> tickets = new List<Ticket>();
            bool seatsFilled = false;
            using (var context = new FlightbookingEntities())
            {
                List<row_details> rowDetails = context.row_details.ToList<row_details>();
                foreach (var row in rowDetails)
                {
                    List<string> seats;
                    if (!String.IsNullOrEmpty(row.seats_filled))
                    {
                        seats = row.seats_filled.Split(',').ToList<string>();
                        for (int i = 1; i <= 7; i++)
                        {
                            if (!seats.Contains(i.ToString()))
                            {
                                Ticket ticket = new Ticket();
                                ticket.applicant_name = name;
                                ticket.row_no = row.row_no;
                                ticket.seat_no = i;
                                row.seats_filled = row.seats_filled + "," + i;
                                context.Ticket.Add(ticket);
                                tickets.Add(ticket);
                                numOfTickets--;
                                if (numOfTickets == 0)
                                {
                                    seatsFilled = true;
                                }
                            }
                        }
                    }
                    if (seatsFilled) {
                        break;
                      }
                              
                }
                context.SaveChanges();
            }



            return tickets;

        }
    }
}