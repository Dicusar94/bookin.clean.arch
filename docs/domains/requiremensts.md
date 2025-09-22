# 📌 DDD Practice Project – Room Booking System

## **Domain**
A small system to manage reservations of rooms in a co-working space.

---

## **Aggregates & Entities**

### **Room (Aggregate Root)**
* **Fields:** RoomId, Name, Capacity, Status (Active/Inactive)
* **Responsibilities:**
    * Prevent overlapping bookings.
    * Can be deactivated (maintenance), which cancels future bookings.

### **Booking (Aggregate Root)**
* **Fields:** BookingId, RoomId, UserId, TimeSlot, Status (Pending/Confirmed/Canceled)
* **Responsibilities:**
    * Booking lifecycle (create, confirm, cancel, reschedule).
    * Ensure valid timeslot (30 min – 8 hrs).
    * Can only cancel before start time.

### **User (Entity, optional external system)**
* **Fields:** UserId, Name, Email

---

## **Value Objects**
* **TimeSlot:** Start, End
    * Invariants: End > Start, Duration ≥ 30 min & ≤ 8 hrs

---

## **Business Invariants**
1. No double-bookings for a room.
2. Booking duration must be valid.
3. Bookings can only be canceled before start time.
4. Bookings must be confirmed within 15 minutes or auto-canceled.

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

---

## **Tables**
1. **Rooms**: RoomId, Name, Capacity, Status
2. **Bookings**: BookingId, RoomId, UserId, StartTime, EndTime, Status
3. **Users** (optional): UserId, Name, Email

---

✅ This setup gives you **2 aggregates, 3 entities, 1 value object, 8 events, and 7 workflows** — enough complexity for a meaningful DDD practice project without being overwhelming.
