class Api::UsersController < ApplicationController

  # POST /api/users
  #
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

  # POST /api/users/1
  #
  def update
    user = User.find(params[:id])
    @status = false

    if user.update(user_params)
      @status = true
      @user = user
    else
      @errors = user.errors.full_messages
    end
  end

  # GET /api/users/1/opponent(/player_xyz)
  #
  def opponent
    @status = false

    begin
      user = User.find(params[:id])
    rescue ActiveRecord::RecordNotFound => e
      @errors = e.message
      render and return
    end


    opponent = User.where(username: params[:name])

    if params[:name] && opponent.exists?
      # Get the opponent by player username
      @status = true
      @opponent = opponent.first

    else
      # Get a random opponent but with an non empty neuronal network
      @opponent = User.where("neuronal_network::text <> '{}'").where.not(id: user.id).sample

      # User.where("neuronal_network is not null").where.not(id: 1).sample
      @status = true
    end
  end

  private
  # Never trust parameters from the scary internet, only allow the white list through.
  #
  def user_params
    params.require(:user).permit(:uuid, :neuronal_network)
  end

end
