json.status @status

if @status
  json.data do
    json.user @opponent
  end

else
  json.errors @errors
end
