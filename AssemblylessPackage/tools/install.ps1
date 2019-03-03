param($installPath, $toolsPath, $package, $project)

$folders = @("Events", "Commands")

$folders |
	? { 
		try {
			return (-not $project.ProjectItems.Item($_))
		}
		catch [System.ArgumentException]
		{
			return $true
		}
	} |
	% { $project.ProjectItems.AddFolder($_, $null) } |
	out-null