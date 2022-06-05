using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace HighRiskDiseaseSurvellance.Infrastructure
{
    public struct ObjectId : IComparable<ObjectId>, IEquatable<ObjectId>
  {
    private static readonly DateTime __unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly long __dateTimeMaxValueMillisecondsSinceEpoch = (DateTime.MaxValue - ObjectId.__unixEpoch).Ticks / 10000L;
    private static readonly long __dateTimeMinValueMillisecondsSinceEpoch = (DateTime.MinValue - ObjectId.__unixEpoch).Ticks / 10000L;
    private static readonly int __staticMachine = ObjectId.GetMachineHash();
    private static readonly short __staticPid;
    private static int __staticIncrement = new Random().Next();
    private readonly int _timestamp;
    private readonly int _machine;
    private readonly short _pid;
    private readonly int _increment;

    static ObjectId() => ObjectId.__staticPid = (short) ObjectId.GetCurrentProcessId();

    /// <summary>Initializes a new instance of the ObjectId class.</summary>
    /// <param name="bytes">The bytes.</param>
    public ObjectId(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      ObjectId.Unpack(bytes, out this._timestamp, out this._machine, out this._pid, out this._increment);
    }

    /// <summary>Initializes a new instance of the ObjectId class.</summary>
    /// <param name="timestamp">The timestamp (expressed as a DateTime).</param>
    /// <param name="machine">The machine hash.</param>
    /// <param name="pid">The PID.</param>
    /// <param name="increment">The increment.</param>
    public ObjectId(DateTime timestamp, int machine, short pid, int increment)
      : this(ObjectId.GetTimestampFromDateTime(timestamp), machine, pid, increment)
    {
    }

    /// <summary>Initializes a new instance of the ObjectId class.</summary>
    /// <param name="timestamp">The timestamp.</param>
    /// <param name="machine">The machine hash.</param>
    /// <param name="pid">The PID.</param>
    /// <param name="increment">The increment.</param>
    public ObjectId(int timestamp, int machine, short pid, int increment)
    {
      if (((long) machine & 4278190080L) != 0L)
        throw new ArgumentOutOfRangeException(nameof (machine), "The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");
      if (((long) increment & 4278190080L) != 0L)
        throw new ArgumentOutOfRangeException(nameof (increment), "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
      this._timestamp = timestamp;
      this._machine = machine;
      this._pid = pid;
      this._increment = increment;
    }

    /// <summary>Initializes a new instance of the ObjectId class.</summary>
    /// <param name="value">The value.</param>
    public ObjectId(string value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      ObjectId.Unpack(ObjectId.ParseHexString(value), out this._timestamp, out this._machine, out this._pid, out this._increment);
    }

    /// <summary>
    ///     Gets an instance of ObjectId where the value is empty.
    /// </summary>
    public static ObjectId Empty { get; }

    /// <summary>Gets the timestamp.</summary>
    public int Timestamp => this._timestamp;

    /// <summary>Gets the machine.</summary>
    public int Machine => this._machine;

    /// <summary>Gets the PID.</summary>
    public short Pid => this._pid;

    /// <summary>Gets the increment.</summary>
    public int Increment => this._increment;

    /// <summary>
    ///     Gets the creation time (derived from the timestamp).
    /// </summary>
    public DateTime CreationTime => ObjectId.__unixEpoch.AddSeconds((double) this._timestamp);

    /// <summary>Compares two ObjectIds.</summary>
    /// <param name="lhs">The first ObjectId.</param>
    /// <param name="rhs">The other ObjectId</param>
    /// <returns>True if the first ObjectId is less than the second ObjectId.</returns>
    public static bool operator <(ObjectId lhs, ObjectId rhs) => lhs.CompareTo(rhs) < 0;

    /// <summary>Compares two ObjectIds.</summary>
    /// <param name="lhs">The first ObjectId.</param>
    /// <param name="rhs">The other ObjectId</param>
    /// <returns>True if the first ObjectId is less than or equal to the second ObjectId.</returns>
    public static bool operator <=(ObjectId lhs, ObjectId rhs) => lhs.CompareTo(rhs) <= 0;

    /// <summary>Compares two ObjectIds.</summary>
    /// <param name="lhs">The first ObjectId.</param>
    /// <param name="rhs">The other ObjectId.</param>
    /// <returns>True if the two ObjectIds are equal.</returns>
    public static bool operator ==(ObjectId lhs, ObjectId rhs) => lhs.Equals(rhs);

    /// <summary>Compares two ObjectIds.</summary>
    /// <param name="lhs">The first ObjectId.</param>
    /// <param name="rhs">The other ObjectId.</param>
    /// <returns>True if the two ObjectIds are not equal.</returns>
    public static bool operator !=(ObjectId lhs, ObjectId rhs) => !(lhs == rhs);

    /// <summary>Compares two ObjectIds.</summary>
    /// <param name="lhs">The first ObjectId.</param>
    /// <param name="rhs">The other ObjectId</param>
    /// <returns>True if the first ObjectId is greather than or equal to the second ObjectId.</returns>
    public static bool operator >=(ObjectId lhs, ObjectId rhs) => lhs.CompareTo(rhs) >= 0;

    /// <summary>Compares two ObjectIds.</summary>
    /// <param name="lhs">The first ObjectId.</param>
    /// <param name="rhs">The other ObjectId</param>
    /// <returns>True if the first ObjectId is greather than the second ObjectId.</returns>
    public static bool operator >(ObjectId lhs, ObjectId rhs) => lhs.CompareTo(rhs) > 0;

    /// <summary>Generates a new ObjectId with a unique value.</summary>
    /// <returns>An ObjectId.</returns>
    public static ObjectId GenerateNewId() => ObjectId.GenerateNewId(ObjectId.GetTimestampFromDateTime(DateTime.UtcNow));

    /// <summary>
    ///     Generates a new ObjectId with a unique value (with the timestamp component based on a given DateTime).
    /// </summary>
    /// <param name="timestamp">The timestamp component (expressed as a DateTime).</param>
    /// <returns>An ObjectId.</returns>
    public static ObjectId GenerateNewId(DateTime timestamp) => ObjectId.GenerateNewId(ObjectId.GetTimestampFromDateTime(timestamp));

    /// <summary>
    ///     Generates a new ObjectId with a unique value (with the given timestamp).
    /// </summary>
    /// <param name="timestamp">The timestamp component.</param>
    /// <returns>An ObjectId.</returns>
    public static ObjectId GenerateNewId(int timestamp)
    {
      int increment = Interlocked.Increment(ref ObjectId.__staticIncrement) & 16777215;
      return new ObjectId(timestamp, ObjectId.__staticMachine, ObjectId.__staticPid, increment);
    }

    /// <summary>
    ///     Packs the components of an ObjectId into a byte array.
    /// </summary>
    /// <param name="timestamp">The timestamp.</param>
    /// <param name="machine">The machine hash.</param>
    /// <param name="pid">The PID.</param>
    /// <param name="increment">The increment.</param>
    /// <returns>A byte array.</returns>
    public static byte[] Pack(int timestamp, int machine, short pid, int increment)
    {
      if (((long) machine & 4278190080L) != 0L)
        throw new ArgumentOutOfRangeException(nameof (machine), "The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");
      if (((long) increment & 4278190080L) != 0L)
        throw new ArgumentOutOfRangeException(nameof (increment), "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
      return new byte[12]
      {
        (byte) (timestamp >> 24),
        (byte) (timestamp >> 16),
        (byte) (timestamp >> 8),
        (byte) timestamp,
        (byte) (machine >> 16),
        (byte) (machine >> 8),
        (byte) machine,
        (byte) ((uint) pid >> 8),
        (byte) pid,
        (byte) (increment >> 16),
        (byte) (increment >> 8),
        (byte) increment
      };
    }

    /// <summary>Parses a string and creates a new ObjectId.</summary>
    /// <param name="s">The string value.</param>
    /// <returns>A ObjectId.</returns>
    public static ObjectId Parse(string s)
    {
      if (s == null)
        throw new ArgumentNullException(nameof (s));
      ObjectId objectId;
      if (ObjectId.TryParse(s, out objectId))
        return objectId;
      throw new FormatException(string.Format("'{0}' is not a valid 24 digit hex string.", (object) s));
    }

    /// <summary>
    ///     Tries to parse a string and create a new ObjectId.
    /// </summary>
    /// <param name="s">The string value.</param>
    /// <param name="objectId">The new ObjectId.</param>
    /// <returns>True if the string was parsed successfully.</returns>
    public static bool TryParse(string s, out ObjectId objectId)
    {
      byte[] bytes;
      if (s != null && s.Length == 24 && ObjectId.TryParseHexString(s, out bytes))
      {
        objectId = new ObjectId(bytes);
        return true;
      }
      objectId = new ObjectId();
      return false;
    }

    /// <summary>
    ///     Unpacks a byte array into the components of an ObjectId.
    /// </summary>
    /// <param name="bytes">A byte array.</param>
    /// <param name="timestamp">The timestamp.</param>
    /// <param name="machine">The machine hash.</param>
    /// <param name="pid">The PID.</param>
    /// <param name="increment">The increment.</param>
    public static void Unpack(
      byte[] bytes,
      out int timestamp,
      out int machine,
      out short pid,
      out int increment)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      if (bytes.Length < 12)
        throw new ArgumentOutOfRangeException(nameof (bytes), "Byte array must be larger 12 bytes long.");
      timestamp = ((int) bytes[0] << 24) + ((int) bytes[1] << 16) + ((int) bytes[2] << 8) + (int) bytes[3];
      machine = ((int) bytes[4] << 16) + ((int) bytes[5] << 8) + (int) bytes[6];
      pid = (short) (((int) bytes[7] << 8) + (int) bytes[8]);
      increment = ((int) bytes[9] << 16) + ((int) bytes[10] << 8) + (int) bytes[11];
    }

    /// <summary>
    ///     Gets the current process id.  This method exists because of how CAS operates on the call stack, checking
    ///     for permissions before executing the method.  Hence, if we inlined this call, the calling method would not execute
    ///     before throwing an exception requiring the try/catch at an even higher level that we don't necessarily control.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int GetCurrentProcessId() => Process.GetCurrentProcess().Id;

    private static int GetMachineHash()
    {
      string machineName = Environment.MachineName;
      byte[] hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(machineName));
      return ((int) hash[0] << 16) + ((int) hash[1] << 8) + (int) hash[2];
    }

    private static int GetTimestampFromDateTime(DateTime timestamp) => (int) Math.Floor((ObjectId.ToUniversalTime(timestamp) - ObjectId.__unixEpoch).TotalSeconds);

    /// <summary>Compares this ObjectId to another ObjectId.</summary>
    /// <param name="other">The other ObjectId.</param>
    /// <returns>
    ///     A 32-bit signed integer that indicates whether this ObjectId is less than, equal to, or greather than the
    ///     other.
    /// </returns>
    public int CompareTo(ObjectId other)
    {
      int num1 = this._timestamp.CompareTo(other._timestamp);
      if (num1 != 0)
        return num1;
      int num2 = this._machine.CompareTo(other._machine);
      if (num2 != 0)
        return num2;
      int num3 = this._pid.CompareTo(other._pid);
      return num3 != 0 ? num3 : this._increment.CompareTo(other._increment);
    }

    /// <summary>Compares this ObjectId to another ObjectId.</summary>
    /// <param name="rhs">The other ObjectId.</param>
    /// <returns>True if the two ObjectIds are equal.</returns>
    public bool Equals(ObjectId rhs) => this._timestamp == rhs._timestamp && this._machine == rhs._machine && (int) this._pid == (int) rhs._pid && this._increment == rhs._increment;

    /// <summary>Compares this ObjectId to another object.</summary>
    /// <param name="obj">The other object.</param>
    /// <returns>True if the other object is an ObjectId and equal to this one.</returns>
    public override bool Equals(object obj) => obj is ObjectId rhs && this.Equals(rhs);

    /// <summary>Gets the hash code.</summary>
    /// <returns>The hash code.</returns>
    public override int GetHashCode() => 37 * (37 * (37 * (37 * 17 + this._timestamp.GetHashCode()) + this._machine.GetHashCode()) + this._pid.GetHashCode()) + this._increment.GetHashCode();

    /// <summary>Converts the ObjectId to a byte array.</summary>
    /// <returns>A byte array.</returns>
    public byte[] ToByteArray() => ObjectId.Pack(this._timestamp, this._machine, this._pid, this._increment);

    /// <summary>Returns a string representation of the value.</summary>
    /// <returns>A string representation of the value.</returns>
    public override string ToString() => ObjectId.ToHexString(ObjectId.Pack(this._timestamp, this._machine, this._pid, this._increment));

    public string ToBase36String() => ObjectId.Pack(this._timestamp, this._machine, this._pid, this._increment).ToBase36String();

    /// <summary>Tries to parse a hex string to a byte array.</summary>
    /// <param name="s">The hex string.</param>
    /// <param name="bytes">A byte array.</param>
    /// <returns>True if the hex string was successfully parsed.</returns>
    public static bool TryParseHexString(string s, out byte[] bytes)
    {
      try
      {
        bytes = ObjectId.ParseHexString(s);
      }
      catch
      {
        bytes = (byte[]) null;
        return false;
      }
      return true;
    }

    /// <summary>
    ///     Parses a hex string into its equivalent byte array.
    /// </summary>
    /// <param name="s">The hex string to parse.</param>
    /// <returns>The byte equivalent of the hex string.</returns>
    public static byte[] ParseHexString(string s)
    {
      if (s == null)
        throw new ArgumentNullException(nameof (s));
      if ((s.Length & 1) != 0)
        s = "0" + s;
      byte[] hexString = new byte[s.Length / 2];
      for (int index = 0; index < hexString.Length; ++index)
      {
        string str = s.Substring(2 * index, 2);
        try
        {
          byte num = Convert.ToByte(str, 16);
          hexString[index] = num;
        }
        catch (FormatException ex)
        {
          throw new FormatException(string.Format("Invalid hex string {0}. Problem with substring {1} starting at position {2}", (object) s, (object) str, (object) (2 * index)), (Exception) ex);
        }
      }
      return hexString;
    }

    /// <summary>Converts a byte array to a hex string.</summary>
    /// <param name="bytes">The byte array.</param>
    /// <returns>A hex string.</returns>
    public static string ToHexString(byte[] bytes) => bytes.ToHexString();

    /// <summary>
    ///     Converts a DateTime to number of milliseconds since Unix epoch.
    /// </summary>
    /// <param name="dateTime">A DateTime.</param>
    /// <returns>Number of seconds since Unix epoch.</returns>
    public static long ToMillisecondsSinceEpoch(DateTime dateTime) => (ObjectId.ToUniversalTime(dateTime) - ObjectId.__unixEpoch).Ticks / 10000L;

    /// <summary>
    ///     Converts a DateTime to UTC (with special handling for MinValue and MaxValue).
    /// </summary>
    /// <param name="dateTime">A DateTime.</param>
    /// <returns>The DateTime in UTC.</returns>
    public static DateTime ToUniversalTime(DateTime dateTime)
    {
      if (dateTime == DateTime.MinValue)
        return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
      return dateTime == DateTime.MaxValue ? DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc) : dateTime.ToUniversalTime();
    }
  }
}