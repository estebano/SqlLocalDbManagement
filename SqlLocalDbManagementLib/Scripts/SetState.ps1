<# sqllocaldb help 
  delete|d ["instance name"]
    Deletes the LocalDB instance with the specified name

  start|s ["instance name"]
    Starts the LocalDB instance with the specified name

  stop|p ["instance name" [-i|-k]]
    Stops the LocalDB instance with the specified name, 
    after current queries finish
    -i request LocalDB instance shutdown with NOWAIT option
    -k kills LocalDB instance process without contacting it
#>
Param(
	$State, 
	$InstanceName, 
	$StoppingOption
)
sqllocaldb $State $InstanceName $StoppingOption