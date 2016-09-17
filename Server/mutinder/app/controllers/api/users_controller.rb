class Api::UsersController < ApplicationController

  def create
    user = User.where(uuid: user_params[:uuid]).first
    @status = false

    if user.present?
      @status = true
      @user = user

    else
      user = User.new(user_params)

      if user.valid?
        user.save
        @user = user
        @status = true
      else
        @errors = user.errors.full_messages
      end
    end
  end

  private
# Never trust parameters from the scary internet, only allow the white list through.
#
def user_params
  params.require(:user).permit(:uuid)
end

end
