# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rails db:seed command (or created alongside the database with db:setup).
#


# #############################################################################
# ONLY IN DEVELOPMENT ENVIRONMENT
# #############################################################################

abort('The Rails environment is NOT running in development mode!') unless Rails.env.development?


ActiveRecord::Base.connection.execute('TRUNCATE TABLE users')


# users #######################################################################
puts 'Seeding users ..........................................................'

100.times do |i|
  user = User.create! do |u|
    u.uuid = "#{rand(1000..9999)}_#{i+1}"
  end
end
