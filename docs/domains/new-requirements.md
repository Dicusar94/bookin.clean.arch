# 📌 DDD Practice Project – Room Booking System

## **Domain**
A system for managing reservations of rooms in a co-working space, supporting schedules, waitlists, and user notifications.

---

## **Aggregates & Entities**

### **Room (Aggregate Root)**
* **Fields:** RoomId, Name, Capacity, Status (Active/Inactive)
* **Responsibilities:**
  * Prevent overlapping bookings.
  * Can be deactivated (maintenance), which cancels future bookings.
  * Maintains schedules defining when bookings are allowed.

#### **RoomSchedule (Entity under Room)**
* **Fields:** ScheduleId, RoomId, DayOfWeek, StartTime, EndTime, IsRecurring, EffectiveDate
* **Responsibilities:**
  * Defines recurring availability (e.g., weekdays 9–18).
  * Supports special date overrides (holidays, maintenance).
  * Ensures bookings occur within valid time windows.

---

### **Booking (Aggregate Root)**
* **Fields:** BookingId, RoomId, UserId, TimeSlot, Status (Pending/Confirmed/Canceled)
* **Responsibilities:**
  * Manage booking lifecycle (create, confirm, cancel, reschedule).
  * Validate that requested timeslot fits within RoomSchedule.
  * Can only cancel before start time.

#### **Waitlist (Entity under Booking/Room)**
* **Fields:** WaitlistId, RoomId, UserId, RequestedTimeSlot
* **Responsibilities:**
  * Stores users waiting for a room when fully booked.
  * Promotes next user when a booking is canceled.

---

### **User (Entity, optional external system)**
* **Fields:** UserId, Name, Email

---

### **Notification (Aggregate Root)**
* **Fields:** NotificationId, UserId, Message, Type (Reminder, Cancellation, Promotion, System), SentDate
* **Responsibilities:**
  * Tracks all user communications.
  * Supports multiple channels (email, SMS, push).
  * Sends reminders and waitlist promotions.

---

## **Value Objects**
* **TimeSlot:** Start, End  
  * **Invariants:** End > Start, Duration ≥ 30 min & ≤ 8 hrs

---

## **Business Invariants**
1. No double-bookings for a room.
2. Booking duration must be valid.
3. Bookings must occur within the room’s schedule availability.
4. Bookings can only be canceled before start time.
5. Bookings must be confirmed within 15 minutes or auto-canceled.
6. Waitlist users can only be promoted if slot becomes available.

---

## **Domain Events**
* BookingPendingConfirmation
* BookingConfirmed
* BookingCreated
* BookingCanceled
* BookingAutoCanceled
* BookingRescheduled
* RoomDeactivated
* ReminderSent
* WaitlistJoined
* WaitlistPromoted
* NotificationSent

---

## **Processes / Workflows**

1. **Create Booking**
   * Command: `CreateBooking(roomId, userId, timeSlot)`  
   * Emits: `BookingPendingConfirmation`

2. **Confirm Booking**
   * Command: `ConfirmBooking(bookingId)`  
   * Emits: `BookingConfirmed`

3. **Auto-Cancel (Timeout Process)**
   * After 15 min → emit `BookingAutoCanceled` if not confirmed.

4. **Cancel Booking**
   * Command: `CancelBooking(bookingId)`  
   * Emits: `BookingCanceled`

5. **Reschedule Booking**
   * Command: `RescheduleBooking(bookingId, newTimeSlot)`  
   * Emits: `BookingRescheduled`

6. **Deactivate Room**
   * Command: `DeactivateRoom(roomId)`  
   * Cancels future bookings → emits `RoomDeactivated` + `BookingAutoCanceled`.

7. **Reminder Process**
   * Sends reminder 1h before booking start.  
   * Emits: `ReminderSent`

8. **Join Waitlist**
   * Command: `JoinWaitlist(roomId, userId, timeSlot)`  
   * Emits: `WaitlistJoined`

9. **Promote from Waitlist**
   * Triggered when a booking is canceled → next waitlisted user is offered the slot.  
   * Emits: `WaitlistPromoted`

10. **Send Notification**
    * Command: `SendNotification(userId, message, type)`  
    * Emits: `NotificationSent`

---

## **Tables**
| Table | Fields |
|--------|---------|
| **Rooms** | RoomId, Name, Capacity, Status |
| **RoomSchedules** | ScheduleId, RoomId, DayOfWeek, StartTime, EndTime, IsRecurring, EffectiveDate |
| **Bookings** | BookingId, RoomId, UserId, StartTime, EndTime, Status |
| **Waitlist** | WaitlistId, RoomId, UserId, RequestedTimeSlot |
| **Users** | UserId, Name, Email |
| **Notifications** | NotificationId, UserId, Message, Type, SentDate |

---

✅ This setup gives you **4 aggregates, 6 entities, 1 value object, 11 events, and 10 workflows** — a balanced DDD design ready for implementation and expansion.
