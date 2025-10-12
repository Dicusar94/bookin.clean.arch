# 📌 Business Invariants – Room Booking System

This document summarizes all **business rules and invariants** for each aggregate in the Room Booking DDD project.

---

## **1️⃣ Room Aggregate**

**Fields:** `RoomId, Name, Capacity, Status`  
**Entities:** `RoomSchedule`

**Business Rules / Invariants:**
- Room must have a **name** and **capacity > 0**.
- Room can only be **Active** or **Inactive**.
- Room **cannot have overlapping schedules**.
- Room schedules must have **StartTime < EndTime**.
- Room can only be booked **within its defined schedule**.
- Deactivating a room cancels **future bookings**.
- Activating a room restores its ability to be booked.
- **When a room schedule changes**, all existing bookings must be revalidated:
  - Bookings outside the new schedule are **automatically canceled**.
  - Users affected must be **notified**.
  - The **waitlist** is re-evaluated for new available slots.

**Domain Events:**
- `RoomDeactivatedEvent`  
- `RoomScheduleChangedEvent`  

**Domain Services Involved:**
- `BookingRevalidationService` – validates and cancels/reschedules affected bookings.  
- `WaitlistReevaluationService` – checks if new slots allow promotions.

---

## **2️⃣ Booking Aggregate**

**Fields:** `BookingId, RoomId, UserId, TimeSlot, Status`  
**Value Object:** `TimeSlot`

**Business Rules / Invariants:**
- **TimeSlot must be valid:** `End > Start`, `Duration ≥ 30min & ≤ 8h`.
- Booking must be **within Room availability**.
- **No overlapping bookings** for the same room.
- Booking **status transitions**:
  - Pending → Confirmed
  - Pending → Canceled (auto or manual)
  - Confirmed → Canceled (before start time only)
- Bookings must be **confirmed within 15 minutes**, otherwise auto-canceled.
- **Cannot cancel or reschedule** after booking has started.
- **Reschedule** must respect availability and no-overlap rules.

**Domain Events:**
- `BookingCreated`  
- `BookingPendingConfirmation`  
- `BookingConfirmed`  
- `BookingCanceled`  
- `BookingAutoCanceled`  
- `BookingRescheduled`  

---

## **3️⃣ Waitlist Aggregate / Entity**

**Fields:** `WaitlistId, RoomId, UserId, RequestedTimeSlot`

**Business Rules / Invariants:**
- Waitlist only exists if **booking is full**.
- Users cannot join waitlist multiple times for **same room & timeslot**.
- Promotion happens **only when a slot becomes available**.
- Waitlist promotion respects **booking rules** (availability, duration).

**Domain Events:**
- `WaitlistJoined`  
- `WaitlistPromoted`  

---

## **4️⃣ Notification Aggregate**

**Fields:** `NotificationId, UserId, Message, Type, SentDate`

**Business Rules / Invariants:**
- Must have **UserId and Message content**.
- Type must be one of: `Reminder, Cancellation, Promotion, System`.
- SentDate is optional until actually sent.
- Notifications can be sent through **multiple channels** (email, SMS, push).
- Tracking ensures **each notification is only sent once**.

**Domain Events:**
- `NotificationSent`  
- `ReminderSent`  

---

## **5️⃣ User Entity**

**Fields:** `UserId, Name, Email`

**Business Rules / Invariants:**
- Must have **unique UserId**.
- Must have **valid email**.
- Name cannot be empty.

> Users may come from an external identity provider.

---

## **6️⃣ TimeSlot Value Object**

**Fields:** `Start, End`

**Business Rules / Invariants:**
- `End > Start`.
- `Duration ≥ 30 minutes && ≤ 8 hours`.
- Optional: must align with **Room schedules** when used in booking.

---

## ✅ Notes

- Start by implementing **Room** and **Booking aggregates** first.  
- Use **domain services** for cross-aggregate rules (e.g., booking overlaps).  
- Raise **domain events** for processes handled outside the aggregate (notifications, waitlist promotions).  
- Implement **Waitlist** and **Notifications** once the basic booking flow works.
