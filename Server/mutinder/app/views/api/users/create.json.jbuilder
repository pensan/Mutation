json.status @status

if @status
  json.data do
    json.user @user
  end

else
  json.errors @errors
end
