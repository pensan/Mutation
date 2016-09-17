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


json_nn = [
    {input: { r: 0.03, g: 0.7, b: 0.5 }, output: { black: 1 }},
    {input: { r: 0.16, g: 0.09, b: 0.2 }, output: { white: 1 }},
    {input: { r: 0.5, g: 0.5, b: 1.0 }, output: { white: 1 }}
]

100.times do |i|
  user = User.create! do |u|
    u.uuid = "#{rand(1000..9999)}_#{i+1}"
    u.neuronal_network = json_nn
  end
end
