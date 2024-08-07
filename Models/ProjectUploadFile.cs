﻿using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class ProjectUploadFile
{
    public int Id { get; set; }

    public int? ProjectId { get; set; }

    public int? FileId { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    public virtual UploadFile? File { get; set; }

    public virtual Project? Project { get; set; }
}
