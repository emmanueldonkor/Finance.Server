using System.Runtime.Serialization;

namespace server.Exceptions;

public class UserNameAlreadyExistExceptions : Exception
{
  public UserNameAlreadyExistExceptions()
  {
  }

  public UserNameAlreadyExistExceptions(string? message) : base(message)
  {
  }

  public UserNameAlreadyExistExceptions(string? message, Exception? innerException) : base(message, innerException)
  {
  }

  protected UserNameAlreadyExistExceptions(SerializationInfo info, StreamingContext context) : base(info, context)
  {
  }
}