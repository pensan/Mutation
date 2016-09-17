class User < ApplicationRecord

  before_save :generate_username

  validates :uuid,
    presence: true,
    uniqueness: {case_sensitive: false}

  # Generates a default username (does not have to be 'unique').
  # If a user logs in with a social account, this username will be replaced.
  #
  def generate_username
    self.username = "player_#{rand(1000..9999)}#{self.id}"
  end

end
