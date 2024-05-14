import win32evtlog
import win32evtlogutil
import win32con

def clear_event_log(log_type):
    # Open the event log
    hand = win32evtlog.OpenEventLog(None, log_type)
    try:
        # Clear the event log
        win32evtlog.ClearEventLog(hand, None)
        print(f"The {log_type} event log has been cleared.")
    finally:
        # Close the event log handle
        win32evtlog.CloseEventLog(hand)

# Specify the log type you want to clear, e.g., 'Application', 'System'
log_to_clear = 'Application'

# Run the function
if __name__ == "__main__":
    clear_event_log(log_to_clear)
