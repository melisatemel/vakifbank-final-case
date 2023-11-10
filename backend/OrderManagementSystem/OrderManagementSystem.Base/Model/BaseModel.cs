using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Base;

public abstract class BaseModel
{
    public int Id { get; set; }
    public int InsertUserId { get; set; }
    public DateTime InsertDate { get; set; }
    public int UpdateUserId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool IsActive { get; set; }
}