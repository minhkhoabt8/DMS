﻿namespace Metadata.Core.Entities;

/// <summary>
/// Represents an account
/// </summary>
public class Account
{
    public Guid ID { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
}