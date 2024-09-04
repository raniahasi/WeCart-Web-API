using System;
using System.Collections.Generic;

namespace Wecartcore.Models;

public partial class ContactU
{
    public int ContactId { get; set; }

    public string UserEmail { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime DateSent { get; set; }

    public bool? IsReplied { get; set; }
}
