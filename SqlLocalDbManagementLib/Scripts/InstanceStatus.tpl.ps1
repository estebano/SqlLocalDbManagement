[CmdletBinding()]
Param(
	[Parameter(Mandatory=$True, Position=1)]
	[string]$InstanceName
)
sqllocaldb i $InstanceName
              